import React from 'react';
import { Form, Button } from 'react-bootstrap';
import AuthService, { ErrorNotification } from '../Services/AuthService';
import { API_URL } from '../Services/ApiService';
import '../styles/Login.scss';

class Login extends React.Component
{

    constructor(props)
    {
        super(props);

        this.state = {
            validationErrors: [],
            email: "test123@gmail.ru",
            password: "TEstPassword91433!!!!432",
            RememberMe: false,
            error: null
        }

        this.onSignIn = this.onSignIn.bind(this);
    }

    onSignIn()
    {
        const RequestBody = {
                                email: this.state.email,
                                password: this.state.password,
                                RememberMe: this.state.RememberMe,
                            };


        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(RequestBody)
        };

        fetch(API_URL + "account/login", requestOptions)
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
            <Form className="form-signin">
                <img src="/img/main-flower.svg" alt="" width="72" height="57"/>

                <h1>Please sign in</h1>   
                
                <Form.Control 
                    size="lg" 
                    type="email" 
                    placeholder="Email address" 
                    value={this.state.email}
                    onChange={(e) => this.setState({email : e.target.value})}/>

                <Form.Control 
                    size="lg" 
                    type="password"
                    placeholder="Password" 
                    value={this.state.password}
                    onChange={(e) => this.setState({password : e.target.value})}/>

                <div className="checkbox">
                    <Form.Check 
                    type="checkbox" 
                    label="Remember me?" 
                    value={this.state.RememberMe} 
                    onChange={() => this.setState({RememberMe : !this.state.RememberMe})}/>
                </div>

                <Button size="lg" variant="primary" onClick={() => this.onSignIn()}>
                    Sign in
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

export default Login;
