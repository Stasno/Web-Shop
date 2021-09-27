import React from 'react';
import AuthService, { ErrorNotification } from '../Services/AuthService';
import { Form, Button } from 'react-bootstrap';
import { API_URL } from '../Services/ApiService';

import '../styles/CreateOrder.scss';

class CreateOrder extends React.Component
{

    constructor(props)
    {
        super(props);

        AuthService.SecuredResource();

        this.state = { 
            phoneNumber: "79234853142",
            zipCode: "123456789",
            address: "Mendileyaef 21/3, 56",
            city: "Leninsk-cuznetsci",
            country: "Russian",
            error: null
        }

        this.onCreateOrder = this.onCreateOrder.bind(this);
    }

    onCreateOrder()
    {
        if (this.state.phoneNumber.length !== 11)
        {
            alert("Phone number must be 11 digits")
            return;
        }

        if (this.state.zipCode.length !== 9)
        {
            alert("Zip code must be 9 digits")
            return;
        }


        const RequestBody = {
                                phoneNumber: this.state.phoneNumber,
                                zipCode: this.state.zipCode,
                                address: this.state.address,
                                city: this.state.city,
                                country: this.state.country,
                            };

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(RequestBody)
        };

        fetch(API_URL + "order/", requestOptions)
            .then(res => 
            {
                if (res.ok === true)
                {
                    this.props.history.replace("/Orders");
                    return res;
                }
                else
                {
                    return Promise.reject(res);
                }
            })
            .catch(err => 
                {
                    ErrorNotification(err);
                    if (err.status === 401)
                    {
                        AuthService.logout();
                    }
                });

        return 1;
    }

    render()
    {
        return(
            <Form className="CreateOrder">
                <img src="/img/main-flower.svg" alt="" width="72" height="57"/>

                <h1>Please enter your address</h1>   
                
                <Form.Control 
                    size="lg" 
                    name="phoneNumber" 
                    placeholder="Phone number" 
                    value={this.state.phoneNumber}
                    onChange={(e) => 
                    {
                        if (e.target.value.length <= 11)
                            this.setState({phoneNumber : e.target.value})}
                    }/>

                <Form.Control 
                    size="lg" 
                    name="zipCode" 
                    placeholder="ZipCode" 
                    value={this.state.zipCode}
                    onChange={(e) => 
                        {
                            if (e.target.value.length <= 9)
                                this.setState({zipCode : e.target.value})}
                        }/>

                <Form.Control 
                    size="lg" 
                    name="address" 
                    placeholder="Address" 
                    value={this.state.address}
                    onChange={(e) => 
                        {
                            if (e.target.value.length < 128)
                                this.setState({address : e.target.value})}
                        }/>

                <Form.Control 
                    size="lg" 
                    name="city" 
                    placeholder="City" 
                    value={this.state.city}
                    onChange={(e) => 
                        {
                            if (e.target.value.length < 128)
                                this.setState({city : e.target.value})}
                        }/>

                <Form.Control 
                    size="lg" 
                    name="country" 
                    placeholder="Country" 
                    value={this.state.country}
                    onChange={(e) => 
                        {
                            if (e.target.value.length < 128)
                                this.setState({country : e.target.value})}
                        }/>


                <Button size="lg" variant="primary" onClick={() => this.onCreateOrder()}>
                    Create order
                </Button>

            </Form>
        );
    }
} 

export default CreateOrder;