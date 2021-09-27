import { Dropdown } from 'react-bootstrap'
import React, { useState} from 'react'

import './styles/Sort.scss'

const items = ['Price', 'Name']
const SortTypes = ["asc", "desc"]

const SortPopup = React.memo(function SortPopup({SelectedSort, onClickSortType, UpdateSort}) {

    const [SelectedItem, setSelectedItem] = useState(SelectedSort)

    return (
    <div className="sort">
        <Dropdown>
            <Dropdown.Toggle variant="primary" id="dropdown-basic">
                {SelectedSort}
            </Dropdown.Toggle>

            <Dropdown.Menu>
                {items.map(name => (
                    
                    SortTypes.map(type => (
                        <Dropdown.Item 
                        key={name + "_" + type} 
                            onClick={() => {
                                        onClickSortType(name.toLowerCase() + "_" + type);
                                        UpdateSort(name + ": " + (type === "asc" ? "Low to high" : "High to low"));
                                        }}>
                            {name + ": " + (type === "asc" ? "Low to high" : "High to low")}
                        </Dropdown.Item>   
                    )) 

                ))}

                <Dropdown.Item
                    key={"None"}
                    onClick={() => {
                        onClickSortType(null); 
                        UpdateSort("Sort");
                        }}>
                    {"Default"}
                </Dropdown.Item>

            </Dropdown.Menu>
        </Dropdown>
    </div>)


});

export default SortPopup;