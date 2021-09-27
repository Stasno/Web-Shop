import React from 'react'
import { Container, Row} from 'react-bootstrap';
import { ApiFetchGet } from '../Services/ApiService';
import {OrderList} from '../components/Orders';
import AuthService from '../Services/AuthService';
import { LoadingBlock } from '../components/Catalog';

import { MyPagination } from '../components';

import '../styles/Orders.scss';


class Orders extends React.Component
{

    FetchOrderItems()
    {
        let Parameters = "?";

        if (this.PageRequest.index && this.PageRequest.index !== 1)
            Parameters += "index=" + this.PageRequest.index + '&';
        
        if (this.PageRequest.limit && this.PageRequest.limit !== 15)
            Parameters += "limit=" + this.PageRequest.limit + '&';

        ApiFetchGet(this.setState.bind(this), "order", Parameters);
    }

    componentDidMount()
    {
        this.FetchOrderItems();
    }

    constructor(props)
    {
        super(props);

        AuthService.SecuredResource();

        this.PageRequest =
        {
            index: 1, 
            limit: 15, 
        }

        var param = new URLSearchParams(this.props.location.search).get("index");
        if (param)
        {
            this.PageRequest.index = param;
        }
        param = new URLSearchParams(this.props.location.search).get("limit");
        if (param)
        {
            this.PageRequest.limit = param;
        }

        this.state = { 
            error: null,
            isLoaded: false,
            RequestResult: {
                currentPage: null, 
                totalPages: null, 
                hasNext: null, 
                hasPrevious: null,
                pageSize: null,
                orders: [],
            }
        }

        this.onChangePage = this.onChangePage.bind(this);

    }

    onChangePage(index)
    {
        if (this.PageRequest.index === index)
            return;
        this.PageRequest.index = index;
        this.FetchOrderItems()
    }

    render()
    {
        const { RequestResult, isLoaded, error } = this.state;

        const { currentPage, totalPages, hasNext, hasPrevious } = RequestResult;

        return(
            <Container className="Orders">
                <Row>
                    <h1 className="heading">
                        Orders
                    </h1>
                </Row>

                <Row>

                    {isLoaded ? <OrderList items={RequestResult.orders}/>
                    : <LoadingBlock/> }

                    {isLoaded && !error &&  RequestResult.pageSize !== 0 ? 
                        <Row className="Pagination">
                            <MyPagination
                            currentPage={currentPage}
                            totalPages={totalPages}
                            hasNext={hasNext}
                            hasPrevious={hasPrevious}
                            onChangePage={this.onChangePage}/>
                        </Row>
                    : <></>
                }
                    
                </Row>
            </Container>
        );
    }


}

export default Orders;