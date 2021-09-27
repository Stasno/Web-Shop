import React, { useState } from 'react';
import {FormControl, Form, Button} from 'react-bootstrap';
import { useHistory } from 'react-router-dom';

import { API_URL } from '../Services/ApiService';

import './styles/SearchField.scss';


function SearchField()
{
    const [items, setItems] = useState([]);
    const [SearchName, setSearchName] = useState("");

    let history = useHistory();

    const fetchSearch = (e) =>
    {
        setSearchName(e.target.value);

        fetch(API_URL + "Catalog/Search?name=" + e.target.value)
        .then(res => res.json())
        .then(
            (result) => {
                setItems(result.items);
            }
        )
    };

    const RedirectToCatalog = () =>
    {
        history.replace("catalog?searchname="+SearchName);
    }

    return (
        <Form className="d-flex search-form">

            <FormControl
                type="search"
                placeholder="Search"
                className="mr-2 item-padding-right"
                aria-label="Search"
                list="browser"
                value={SearchName}
                onChange={fetchSearch}
                />

            <datalist id="browser">
                {items.map(item => <option id={item.id}>{item.title}</option>)}
            </datalist>

            <Button className="button" variant="outline-success" onClick={RedirectToCatalog}>Search</Button>
        </Form>
       
    );
}

export default SearchField;