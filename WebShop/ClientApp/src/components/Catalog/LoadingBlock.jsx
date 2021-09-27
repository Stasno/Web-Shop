import React from 'react'
import { Col, Spinner } from 'react-bootstrap';

const LoadingBlock =  function LoadBlock(props) 
{
    return (
        <Col xs="12" style={{display:"flex", justifyContent:"center"}}>
            <Spinner animation="border"/>
        </Col>
    );    
}

export default LoadingBlock;