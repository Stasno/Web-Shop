import React from 'react'
import { Pagination, Col } from 'react-bootstrap';

function getAccessiblePages(first, last, position) {
    if (position > last) {
        position = last;
    }
    if (position < first) {
        position = first;
    }

    if (last - first < 5) {
        let result = [];
        while (first <= last) {
            result.push(first++);
        }
        return result;
    }

    let pageCount = 4;
    let result = [position];
    let i = 1;

    while (pageCount) {
        if (position - i >= first) {
            result.unshift(position - i);
            pageCount--;
        }

        if (position + i <= last) {
            result.push(position + i);
            pageCount--;
        }
        i++;
    }

    return result;
}

function MyPagination({ currentPage, totalPages, hasNext, hasPrevious, onChangePage }) {
    let AccessiblePages = getAccessiblePages(1, totalPages, currentPage)

    return (
        <>
            <Col xs="3"></Col>
            <Col xs="6" >
                <Pagination>

                    <Pagination.Item
                        disabled={currentPage <= 1}
                        onClick={() => onChangePage(1)}
                        key="<<">
                        {"<<"}
                    </Pagination.Item>
                    <Pagination.Item
                        disabled={currentPage <= 1}
                        onClick={() => onChangePage(currentPage - 1)}
                        key="<">
                        {"<"}
                    </Pagination.Item>

                    {
                        AccessiblePages.map((item, index) =>
                            <Pagination.Item
                                onClick={() => onChangePage(item)}
                                active={item === currentPage} activeLabel=""
                                key={index}>
                                {item}
                            </Pagination.Item>)
                    }

                    <Pagination.Item
                        disabled={currentPage >= totalPages}
                        onClick={() => onChangePage(currentPage + 1)}
                        key=">">
                        {">"}
                    </Pagination.Item>
                    <Pagination.Item
                        disabled={currentPage >= totalPages}
                        onClick={() => onChangePage(totalPages)}
                        key=">>">
                        {">>"}
                    </Pagination.Item>

                </Pagination>
            </Col>
        </>
    );
}

export default MyPagination;