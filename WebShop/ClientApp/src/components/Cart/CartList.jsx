import React from 'react';
import CartItem from './CartItem';
import { Row } from 'react-bootstrap';
import './styles/CartList.scss';



function CartList({items, decFunc, incFunc, removeFunc})
{
    return(
        <Row className="CartList">
            {items.map((item, index) => {
                return <CartItem  
                item={item} 
                decFunc={() => decFunc(index)} 
                incFunc={() => incFunc(index)}
                removeFunc={() => removeFunc(index)}
                inStock={item.inStock}
                key={item.id}/>
            })}
        </Row>
    );
}

export default CartList;