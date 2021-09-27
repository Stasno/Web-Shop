import React from 'react';
import { Button, Col} from 'react-bootstrap';
import { Link } from 'react-router-dom';

import { API_URL } from '../../Services/ApiService';
import { ErrorNotification } from '../../Services/AuthService';

import './styles/ProductItem.scss';


function AddToCart(id)
{
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' }
    };

    fetch(API_URL + "cart/" + id, requestOptions)
        .then(res => 
            {
                if (res.ok)
                {
                    return res;
                }
                return Promise.reject(res);
                
            })
        .then(
            (res) => {
                
            }
        )
        .catch(err => 
            {
                ErrorNotification(err);
            }
        );
}

const ProductItem = function ProductList({item})
{
    return (
        <Col sm="12" md="6" lg="4" xs="12" className="ProductItem">
            <li className="ProductContent">

                    
                <img
                        
                    src={item.imghref}
                    height="250"
                    width="250"  
                    className="itemImg"
                alt={item.title} />


                <div className="itemTitle">
                    {item.title}
                </div>

                <div className="itemPrice">
                    {item.price}
                    <span className="dollar"> $ </span>
                </div>

                <form>
                    <Button 
                        disabled={item.inStock ? false : true}
                        variant={item.inStock ? "outline-success" : "outline-danger"}
                        className="btn"
                        onClick={() => AddToCart(item.id)}>
                        {item.inStock ? "Add to cart" : "Sold out"}
                    </Button>
                </form>

            </li>
        </Col>
        
    );
}

export default ProductItem;