import React from 'react';
import { Accordion, Col, Row } from 'react-bootstrap';
import OrderItem from './OrderItem';

import './styles/OrderList.scss'

function OrderList({items})
{
    return (
        <Accordion defaultActiveKey="0" className="Accordion">
            <Row>
                <Col lg="1"></Col>
                <Col lg="10">
                    {items.map((order, index) => 
                    {
                        return <OrderItem order={order} index={index}/>
                    })}
                </Col>
                <Col lg="1"/>
            </Row>
        </Accordion>
    );
}

export default OrderList;
