import React from 'react';
import { Form, Button } from 'react-bootstrap';
import AuthService from '../Services/AuthService';
import { API_URL } from '../Services/ApiService';

import '../styles/Register.scss';

class Register extends React.Component
{

    constructor(props)
    {
        super(props);

        this.state = { 
            validationErrors: [],
            email: "",
            password: "",
            passwordConfirm: "",
            firstname: "",
            secondname: "",
            error: null
        }
        this.onSignUp = this.onSignUp.bind(this);
    }

    onSignUp()
    {
        const RequestBody = {
                                email: this.state.email,
                                password: this.state.password,
                                firstname: this.state.firstname,
                                secondname: this.state.secondname,
                                passwordConfirm: this.state.passwordConfirm,
                            };

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(RequestBody)
        };

        fetch(API_URL + "account/register", requestOptions)
            .then(res => 
            {
                if (res.ok)
                {
                    return res;
                }
                else
                {
                    return Promise.reject(res);
                }
            })
            .then(
                (res) => {
                    AuthService.login();
                    this.props.history.replace("/catalog");
                }
            )
            .catch(err => 
                {
                    if (err.status === 400)
                    {
                        err.json().then(msg => 
                            {   
                                if (msg.errors)
                                {
                                    let errors = [];
                                    for (let i in msg.errors)
                                    {
                                        errors.push(msg.errors[i]);
                                    }

                                    this.setState({validationErrors: errors})

                                    return;
                                }
                            });
                    };
                });

        return 1;
    }

    render()
    {
        return (
            <Form className="form-signup">
                
                <img src="/img/main-flower.svg" alt="" width="72" height="57"/>

                <h1>Please sign up</h1>  

                <Form.Control size="lg" 
                name="email" 
                placeholder="Email address"
                value={this.state.email}
                onChange={(e) => this.setState({email : e.target.value})}/>

                <Form.Control size="lg" 
                name="Firstname" 
                placeholder="Firstname"
                value={this.state.firstname}
                onChange={(e) => this.setState({firstname : e.target.value})}/>

                <Form.Control size="lg" 
                name="Secondname" 
                placeholder="Secondname"
                value={this.state.secondname}
                onChange={(e) => this.setState({secondname : e.target.value})}/>

                <Form.Control size="lg" 
                name="password" 
                placeholder="Password"
                value={this.state.password}
                onChange={(e) => this.setState({password : e.target.value})}/>

                <Form.Control size="lg" 
                name="PasswordConfirm" 
                placeholder="PasswordConfirm"
                value={this.state.passwordConfirm}
                onChange={(e) => this.setState({passwordConfirm : e.target.value})}/>

                <Button size="lg" variant="primary" onClick={() => this.onSignUp()}>
                    Sign up
                </Button>

                <div className="errors">
                    {this.state.validationErrors.map(i =>
                        {
                            return (
                                <div className="error">
                                    {i}
                                </div>
                            );
                        })}
                </div>


            </Form>
        );
    }

}

export default Register;