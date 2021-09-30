import React from 'react';
import { Button, Col, Container, Row } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import CartList from '../components/Cart/CartList';
import { ApiFetchGet, API_URL } from '../Services/ApiService';
import AuthService from '../Services/AuthService';
import { LoadingBlock } from '../components/Catalog';

import '../styles/Cart.scss';




class Cart extends React.Component
{

    CartRequest(method, request, body)
    {
        const requestOptions = {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        };

        fetch(API_URL + "cart/" + request, requestOptions)
            .then(res => 
                {
                    if (res.ok)
                    {
                        return res.json();
                    }
                    else
                    {
                        if (res.status === 401)
                        {
                            AuthService.logout();
                        }
                        Promise.reject(res);
                    }
                })
            .then(res => 
                {
                    if (method === "PUT")
                    {
                        let i = this.state
                            .RequestResult
                            .items
                            .findIndex(i => i.id === request);

                        if (res.inStock === false)
                        {
                            if (this.state.RequestResult.items[i].inStock !== res.inStock)
                                this.setState({IsButtonAvailable: this.state.IsButtonAvailable + 1});
                        }
                        else
                        {
                            if (this.state.RequestResult.items[i].inStock !== res.inStock && this.state.IsButtonAvailable - 1 >= 0)
                                this.setState({IsButtonAvailable: this.state.IsButtonAvailable - 1});
                        }

                        this.state.RequestResult.items[i].inStock = res.inStock;
                        
                        this.setState({RequestResult: this.state.RequestResult});

                        
                            

                        return Promise.resolve();
                    }
                },
                err =>
                {
                    return Promise.resolve();
                }
            )
            .catch(err => 
                {
                    console.log("error cart", err);
                });
    }


    FetchCartItems()
    {
        //ApiFetchGet(this.setState.bind(this), "Cart");
        ApiFetchGet((state) => 
        {
            this.setState(state);
            let count = 0;
            state.RequestResult.items.forEach(i =>
                {
                    if (i.inStock === false)
                        count++;
                });

            this.setState({IsButtonAvailable: count});
        }, "Cart");
    }

    constructor(props)
    {
        super(props);

        AuthService.SecuredResource();

        this.state={
            IsButtonAvailable: 0,// 0 - true, variable > 0 - false
            isLoaded: false,
            RequestResult: 
            {
                size: null,
                items: []
            },
        };


        this.RemoveItem = this.RemoveItem.bind(this);
        this.DecItemQuantity = this.DecItemQuantity.bind(this);
        this.IncItemQuantity = this.IncItemQuantity.bind(this);
        this.CartRequest = this.CartRequest.bind(this);

    }

    componentDidMount()
    {
        this.FetchCartItems();
    }


    RemoveItem(index)
    {
        const { RequestResult } = this.state;

        this.CartRequest("DELETE", RequestResult.items[index].id);

        if (RequestResult.items[index].inStock === false)
        {
            this.setState({IsButtonAvailable: this.state.IsButtonAvailable - 1});
        }

        RequestResult.items.splice(index, 1);
        
        this.setState({ 
            RequestResult 
        });
    }

    EmptyCart()
    {
        this.CartRequest("DELETE", "");

        const { RequestResult } = this.state;

        RequestResult.items.splice(0, RequestResult.items.length);
        this.setState({ 
            RequestResult,  
            IsButtonAvailable: 0
        });
    }

    DecItemQuantity(index)
    {
        
        const { RequestResult } = this.state;

        if (RequestResult.items[index].quantity - 1 < 1)
        {
            return;
        }

        RequestResult.items[index].quantity -= 1;

        this.CartRequest("PUT", RequestResult.items[index].id, RequestResult.items[index].quantity);

        this.setState({
            RequestResult: RequestResult
        });

    }

    IncItemQuantity(index)
    {
        const { RequestResult } = this.state;

        RequestResult.items[index].quantity += 1;

        this.CartRequest("PUT", RequestResult.items[index].id, RequestResult.items[index].quantity);

        this.setState({
            RequestResult: RequestResult
        });
    }

    render()
    {

        const {isLoaded, RequestResult, IsButtonAvailable} = this.state;

        console.log("render ", IsButtonAvailable);

        

        return(
            <Container className="cart">
                <Row>
                    <h1 className="heading">
                        cart
                    </h1>
                </Row>
                
                {isLoaded ? 
                    <>
                        {RequestResult.items.length > 0 ?
                            <>
                                <CartList 
                                    items={RequestResult.items}
                                    decFunc={this.DecItemQuantity} 
                                    incFunc={this.IncItemQuantity} 
                                    removeFunc={this.RemoveItem}/>
                                    

                                <Row>
                                    <Col xs="1" sm="0" lg="1" xl="2"/>
                                    <Col xs="10" sm="12" lg="10" xl="8"
                                        className="cartInteract">
                                        <Col xs="3">
                                            <a href = "javascript:void(0);"
                                            onClick={() => this.EmptyCart()} 
                                            className="emptyCart">
                                                Remove all items
                                            </a>
                                        </Col>
                                        <Col xs="9">
                                            <div className="totalPrice">
                                                Total Price: 
                                                <div className="price">
                                                    {
                                                        (() => 
                                                        {
                                                            let sum = 0;
                                                            RequestResult.items.forEach(i =>
                                                                {
                                                                sum += i.quantity * i.price; 
                                                                });
                                                            return sum;
                                                        })()
                                                    }
                                                </div>
                                                <span className="dollar"> $ </span>
                                            </div>
                                        </Col>
                                    </Col>
                                    <Col xs="1" sm="0" lg="1" xl="2"/>
                                </Row>

                                <Row>

                                    <Col xs="1" sm="0" lg="1" xl="2"/>

                                    <Col xs="10" sm="12" lg="10" xl="8">

                                        <Link to="/createOrder" className="createOrder">
                                            <Button 
                                            variant="success" 
                                            disabled={IsButtonAvailable > 0 ? "false" : ""}>
                                                Create Order
                                            </Button>
                                        </Link>
                                        
                                    </Col>

                                    <Col xs="1" sm="0" lg="1" xl="2"/>
                                </Row>
                        </>
                        : <div className="emptyCart">Your cart is empty</div>
                        }
                    </>
                : <LoadingBlock/> }

            </Container>
        );
    }


}

export default Cart;