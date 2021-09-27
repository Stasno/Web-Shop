import React from 'react'

import ProductItem from './ProductItem';

import './styles/ProductList.scss'

const ProductList = function ProductList({items})
{
    return (
        <div className="ProductList">
                <ul>
                    {items.map(item => 
                        {
                            return (<ProductItem key={item.id} item={item}/>);
                        })}
                </ul>
        </div>
        
        
    );
}

export default ProductList;