import React from 'react'
import { 
    Button, 
    Col,
    Row 
} from 'react-bootstrap';
import { Link } from 'react-router-dom';

import './styles/CartItem.scss'

function CartItem({item, decFunc, incFunc, removeFunc, inStock})
{
    return(
        <>
            <Col xs="0" lg="1" xl="2"/>
                <Col xs="12" lg="10" xl="8"  className={"CartItem" + (!inStock ? " OutOfStock" : " ")}>

                    {!inStock &&
                        <div className="titleOut">
                            Out of stock
                        </div>
                    }

                    <Row>
                        <Col xs="2" className="img">
                            <img src={item.imghref} alt={item.title} width="100" height="100"></img>
                        </Col>

                        <Col xs="4" className="title">
                            {item.title} 
                        </Col>

                        <Col xs="3" className="quantity">

                            <Button 
                            className="decbtn" 
                            onClick={() => decFunc()}>
                                -
                            </Button>

                            <Link className="number">
                                {item.quantity}
                            </Link>

                            <Button 
                            className="incbtn" 
                            onClick={() => incFunc()}>
                                +
                            </Button>
                            

                        </Col>

                        <Col xs="2" className="price">
                            <div>
                            {item.quantity * item.price}
                            <span className="dollar"> $ </span>
                            </div>
                        </Col>

                        <Col xs="1" className="removeItem">
                            <Button 
                            variant="true" 
                            onClick={() => removeFunc()}>
                                X
                            </Button>
                        </Col>


                    </Row>
                </Col>
            <Col xs="0" lg="1" xl="2"/>
        </>
    );
}
//      ₽
export default CartItem;