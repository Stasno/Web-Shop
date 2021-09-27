import React from 'react';
import { Container, Row, Col} from 'react-bootstrap';

import { 
    Categories, 
    ProductList, 
    SortPopup, 
    LoadingBlock 
}   from '../components/Catalog';

import { MyPagination } from '../components';

import { ApiFetchGet } from '../Services/ApiService';

import "../styles/Catalog.scss";

const CategoryList = ['Roses', 'Tulips', 'Irises']

class Catalog extends React.Component
{

    FetchProducts()
    {
        let Parameters = "?";                       //  "https://localhost:3000/api/Catalog/GetPage";
        if (this.PageRequest.category)
            Parameters += "Category=" + this.PageRequest.category + '&';

        if (this.PageRequest.sort)
            Parameters += "Sort=" + this.PageRequest.sort + '&';

        if (this.PageRequest.searchName)
            Parameters += "SearchName=" + this.PageRequest.searchName + '&';

        if (this.PageRequest.index && this.PageRequest.index !== 1)
            Parameters += "index=" + this.PageRequest.index + '&';
        
        if (this.PageRequest.limit && this.PageRequest.limit !== 12)
            Parameters += "limit=" + this.PageRequest.limit + '&';

        ApiFetchGet(this.setState.bind(this), "catalog", Parameters);

        this.props.history.push({
            search: Parameters
        });
        
    }

    constructor(props)
    {
        super(props);

        props.location.search = props.location.search.toLowerCase();

        this.PageRequest =
        {
            sort: new URLSearchParams(this.props.location.search).get("sort"), 
            category: new URLSearchParams(this.props.location.search).get("category"), 
            searchName: new URLSearchParams(this.props.location.search).get("searchname"), 
            index: 1, 
            limit: 12, 
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
            SelectedSort: "Sort",
            error: null,
            isLoaded: false,
            RequestResult: {
                currentPage: null, 
                totalPages: null, 
                hasNext: null, 
                hasPrevious: null,
                pageSize: null,
                products: []
            }
        }

        this.onChangeCategory = this.onChangeCategory.bind(this);
        this.onChangeSort = this.onChangeSort.bind(this);
        this.FetchProducts = this.FetchProducts.bind(this);
        this.onChangePage = this.onChangePage.bind(this);
    }

    componentDidMount() 
    {
        this.FetchProducts();
    }

    componentDidUpdate(prevProps) {
        console.log(prevProps.location, this.props.location, prevProps.location.search.length);

        const update = () =>
        {
            this.PageRequest.sort = new URLSearchParams(prevProps.location.search).get("sort");
            this.PageRequest.category = new URLSearchParams(prevProps.location.search).get("category");
            this.PageRequest.searchName = new URLSearchParams(prevProps.location.search).get("searchname");
            this.PageRequest.index = 1;
            this.setState({
                SelectedSort: 
                (this.PageRequest.sort==null ? 
                    "sort" : 
                    this.PageRequest.sort)
            });
            this.FetchProducts();
        };

        if (this.props.location.search == "")
        {
            update();
            return;
        }

        if (new URLSearchParams(this.props.location.search).get("searchname") && this.props !== prevProps) {
            this.PageRequest.searchName=new URLSearchParams(this.props.location.search).get("searchname");
            this.FetchProducts();
        }
    }

    onChangeCategory(category)
    {
        if (this.PageRequest.category === category)
            return;
        this.PageRequest.index = 1;
        this.PageRequest.category = category;
        this.FetchProducts();
    }

    onChangeSort(sort)
    {
        if (this.PageRequest.sort === sort)
            return;
        this.PageRequest.index = 1;
        this.PageRequest.sort = sort;
        this.FetchProducts()
    }

    onChangePage(index)
    {
        if (this.PageRequest.index === index)
            return;
        this.PageRequest.index = index;
        this.FetchProducts()
    }

    render()
    {
        const { error, isLoaded, RequestResult, SelectedSort } = this.state;

        const { currentPage, totalPages, hasNext, hasPrevious } = RequestResult;
 
        return (
            <Container>
                <Row>
                    <Col lg="10" md="10">
                        <Categories items={CategoryList} onClickCategory={this.onChangeCategory}/>
                    </Col>
                    <Col lg="2" md="10">
                        <SortPopup 
                        SelectedSort={SelectedSort} 
                        onClickSortType={this.onChangeSort}
                        UpdateSort={(name) => {this.setState({SelectedSort: name})}}/>
                    </Col>
                </Row>

                <Row>
                    <Col xs="3"></Col>
                    <Col xs="6" >
                        <h1 className="heading">
                            Catalog
                        </h1>
                    </Col>

                </Row>

                <Row style={ { paddingTop:'20px' }  }>
                    
                    {isLoaded ? <ProductList items={RequestResult.products}/> 
                    : <LoadingBlock/> }
                    
                </Row>

                {isLoaded && !error &&  RequestResult.pageSize !== 0? 
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

            </Container>

        );
    }


}


export default Catalog;