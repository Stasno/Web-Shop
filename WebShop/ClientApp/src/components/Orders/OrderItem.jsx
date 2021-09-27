import React from 'react';
import { Accordion, Col, Row } from 'react-bootstrap';

import './styles/OrderItem.scss'

function OrderItem({order, index})
{
    return (
        <Accordion.Item eventKey={index}>
            <Accordion.Header className="accordionHeader">

                <Col xs="3" lg="2" className="text">
                    <div className="divWrap">
                        Order №&ensp; 
                        <span className="secondText">
                            {order.id}
                        </span>
                    </div>
                </Col>

                <Col xs="3" className="text">
                    <div className="divWrap">
                        Placed at:&ensp;
                        <span className="secondText">
                            {order.placedAt}
                        </span>
                    </div>
                </Col>

                <Col xs="3" className="text">
                    <div className="divWrap">
                        Total price: &ensp;
                        <span className="secondText">
                            {order.totalPrice}
                            <span className="dollar"> $ </span>
                        </span>
                    </div>
                </Col>

                <Col xs="2" lg="3" className="text">
                    <div className="divWrap">
                        Status:&ensp; 
                        <span className="secondText">
                            {order.orderStatus}
                        </span>
                    </div>
                </Col>

            </Accordion.Header>
            <Accordion.Body>
                <Row>
                    <Col xs="6" className="field">
                        Zip code:&ensp;
                        <div className="data">
                            {order.zipCode}
                        </div>
                    </Col>
                    <Col xs="6" className="field">
                        Address:&ensp;
                        <div className="data">
                            {order.address}
                        </div>
                    </Col>
                    <Col xs="6" className="field">
                        City:&ensp;
                        <div className="data">
                            {order.city}
                        </div>
                    </Col>
                    <Col xs="6" className="field">
                        Country:&ensp;
                        <div className="data">
                            {order.country}
                        </div>
                    </Col>
                    <Col xs="6" className="field">
                        Placed at: &ensp;
                        <div className="data">
                            {order.placedAt}
                        </div>
                    </Col>
                    <Col xs="6" className="field">
                        Last update:&ensp;
                        <div className="data">
                            {order.lastUpdate}
                        </div>
                    </Col>
                </Row>


                <Row className="tableHead">
                    <Col xs="2" className="text"
                        style={{borderLeft: "none"}}>
                    </Col>

                    <Col xs="4" className="text">
                        Title 
                    </Col>

                    <Col xs="3" className="text">
                        Quantity
                    </Col>

                    <Col xs="3" className="text">
                        Price
                    </Col>    
                </Row>

                {order.items.map((item, index) => 
                    {
                        return (
                            <Row className={"productItem " + 
                            (index === order.items.length-1 ? "last" : "")}>
                                <Col xs="2" className="img">
                                    <img src={item.imghref} 
                                    alt={item.title} 
                                    width="100" 
                                    height="100"
                                    className="image"></img>
                                </Col>

                                <Col xs="4" className="title">
                                    {item.title} 
                                </Col>

                                <Col xs="3" className="quantity">

                                    <div className="number">
                                        {item.quantity}
                                    </div>
                                    

                                </Col>

                                <Col xs="3" className="price">
                                    <div>
                                    {item.quantity * item.price}
                                    <span className="dollar"> $ </span>
                                    </div>
                                </Col>
                            </Row>
                        );
                    })}
            </Accordion.Body>
        </Accordion.Item>
    );
}

export default OrderItem;
