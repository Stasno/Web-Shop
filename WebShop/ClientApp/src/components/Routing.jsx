import React from 'react';
import { Route, Switch } from 'react-router';

import {
    Cart, 
    Catalog, 
    Login, 
    Register,
    CreateOrder,
    Orders
} from '../pages'
import AuthService from '../Services/AuthService';

export default class Routing extends React.Component
{

    render() {
            
        const ShouldBeAuthenticated = (component) =>
        {
            if (AuthService.IsAuthenticatedFast() === true)
            {
                return component;
            }
            else
            {
                return Login;
            }
        }

        return (
            <>
                <main>
                    <Switch>
                        <Route strict exact path={["/", "/Catalog"]} 
                        component={Catalog}/>

                        <Route strict exact path="/login" 
                        component={Login}/>

                        <Route strict exact path="/register" 
                        component={Register}/>

                        <Route strict exact path="/cart" 
                        component={ShouldBeAuthenticated(Cart)}/>

                        <Route strict exact path="/CreateOrder" 
                        component={ShouldBeAuthenticated(CreateOrder)}/>

                        <Route strict exact path="/Orders" 
                        component={ShouldBeAuthenticated(Orders)}/>

                        <Route>
                            <div>NoMatch</div>
                        </Route>

                    </Switch>
                </main>
                <footer>
                </footer>
            </>
        );
    }

}