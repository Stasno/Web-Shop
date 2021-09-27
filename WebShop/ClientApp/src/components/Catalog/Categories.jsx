import React from 'react'

import './styles/Categories.scss'

const Categories = function ({activeCategory, items, onClickCategory})
{
    return (
        <div className="Categories">
            <ul>
                <li
                    className={activeCategory === null ? 'btn active' : 'btn'}
                    onClick={() => onClickCategory(null)}>
                    All
                </li>
                {items && items.map((name, index) => (
                    <li
                    className={activeCategory === index ? 'btn active' : 'btn'}
                    onClick={() => onClickCategory(name)}
                    key={index}>
                    {name}
                    </li>
                ))}
            </ul>
        </div>
    );
};

Categories.defaultProps = { activeCategory: null, items: [] };

export default Categories;