
const Constant = {
    operators: {
        string: {
            "eq": 'Equals',
            'neq': 'Not Equal',
            'contains': 'Contains',
            'startsWith': 'Start With',
            'endsWith': 'End With',
            'isNull': 'Empty',
            'isNotNull': 'Not Empty'
        },
        number: {
            "eq": 'Equals To',
            'neq': 'Not Equal To',
            'lt': 'Less Than',
            'gt': 'Greater Than',
            'lteq': 'Less Than or Equal to',
            'gteq': 'Greater Than or Equal to'
        },
        datetime: {
            "eq": 'Equals To',
            'neq': 'Not Equal To',
            'lt': 'Less Than',
            'gt': 'Greater Than',
            'lteq': 'Less Than or Equal to',
            'gteq': 'Greater Than or Equal to'
        },
        boolean: {
            "eq": 'Equals To'
        }        
    },
    values: {
        boolean: {
            0: 'False',
            1: 'True'
        },
        logic: {
            1: 'And',
            2: 'Or'
        }
    }
}



//create Modal when clicking filter button for user can create filter item on table
var FilterModal = (function () {
    var _id, _columns;
    //function FilterModal(id, columns) {
    //}

    function _createModal(id, columns) {
        _id = id;
        _columns = columns;
        var html = `<div class="modal modal-xl fade" id="${id}-FilterModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                      <div class="modal-dialog" role="document">
                        <div class="modal-content">
                          <div class="modal-header">
                            <h5 class="modal-title text-primary-d3" id="exampleModalLabel">
                              Modal title
                            </h5>

                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                              <span aria-hidden="true">&times;</span>
                            </button>
                          </div>

                          <div class="modal-body">
                          </div>

                          <div class="modal-footer">
                            <button type="button" class="btn btn-secondary px-4" data-dismiss="modal">
                              Close
                            </button>

                            <button type="button" class="btn btn-primary">
                              Save changes
                            </button>
                          </div>
                        </div>
                      </div>
                    </div>`;
        $(`#modal`).empty();
        $(`#modal`).append(html);
        //_createBody();
        _createFilterBlock(0);
        $(`#${_id}-FilterModal`).modal('show');
    }
    function _createBody() {
        var $body = $(`#${_id}-FilterModal .modal-body`);
        var index = $body.children().children().length;
        $(`#${_id}-FilterModal .modal-body`).append(_createFilterBlock(index));

        
    }
    function _setListener(index) {
        var column = `#${_id}-filter-column-${index}`;
        var operator = `#${_id}-filter-operator-${index}`;
        var value = `#${_id}-filter-value-${index}`;
        var logic = `#${_id}-filter-logic-${index}`;
        var newBtn = `#${_id}-filter-new-${index}`;
        var removeBtn = `#${_id}-filter-remove-${index}`;

        $(column).on('change', function () {
            var val = $(this).val();
            var type = val.split('_')[1];
            var colName = val.split('_')[0];
            var operators = Constant.operators[type];

            $(operator).find('option').not(':first').remove();
            if (type == 'boolean') {
                let $select = $(`<select id="${_id}-filter-value-${index}" class="form-control">`);
                for (let p in Constant.values.boolean) {
                    $select.append(`<option value="${p}">${Constant.values.boolean[p]}</option>`);                    
                }
                $(value).replaceWith($select);
            }
            else {
                let replace = $(`<input type="text" id="${_id}-filter-value-${index}" class="form-control">`);
                $(value).replaceWith(replace);
            }
            for (let p in operators) {
                $(operator).append(`<option value="${p}">${operators[p]}</option>`)
            }
        });

        $(operator).on('change', function () {
            var val = $(this).val();
            if (val == 'isNull' || val == 'isNotNull') {
                $(value).closest('div').css('display', 'none');
            }
            else {
                $(value).closest('div').css('display', 'block');
            }
        })

        $(newBtn).on('click', function () {
            let idx = $(`#${_id}-FilterModal .modal-body`).children().length;
            $(`#${_id}-FilterModal .modal-body`).append(_createFilterBlock(idx));
        })
        $(removeBtn).on('click', function () {
            //$(`#${_id}-FilterModal .modal-body`).append(_createFilterBlock(index + 1));
            $(this).closest('div .form-row').remove();
        })
    }
    function _createFilterBlock(index) {
        var styleCss = index > 0 ? 'style="margin-top:1rem";' : ''
        var $row = $(`<div class="form-row" ${styleCss}">`);

        var $columnFilter = $(_createColumnFilter(index));
        var $operatorFilter = $(_createOperatorFilter(index));
        var $valueFilter = $(_createValueFilter(index));
        var $logicFilter = $(_createLogicFilter(index));
        var $actionFilter = $(_createActionFilter(index));

        $row.append($columnFilter);
        $row.append($operatorFilter);
        $row.append($valueFilter);
        $row.append($logicFilter);
        $row.append($actionFilter);

        var $body = $(`#${_id}-FilterModal .modal-body`);
        //var index = $body.children().length;
        $(`#${_id}-FilterModal .modal-body`).append($row);
        _setListener(index);
        //return $row;
    }
    function _createColumnFilter(index) {

        $select = $(`<select class="form-control" id="${_id}-filter-column-${index}"><option>Please select column</option></select>`);
        for (let i = 0; i < _columns.length; i++) {
            let col = _columns[i];
            let type = col.type;
            let field = col.field;
            let title = col.title;
            if (field != 'state' && field != 'tools')
                $select.append(`<option value="${field}_${type}">${title}</option>`);
        }
        return $(`<div class="col-md-3">`).append($select);
    }
    function _createOperatorFilter(index) {
        $select = $(`<select class="form-control" id="${_id}-filter-operator-${index}"><option>Please select operator</option></select>`);
        return $(`<div class="col-md-3">`).append($select);
        //return $select;
    }
    function _createValueFilter(index) {
        $input = $(`<input class="form-control" id="${_id}-filter-value-${index}" type="text" placeholder="Please type value">`);
        return $(`<div class="col-md-3">`).append($input);
        //return $input
    }
    function _createLogicFilter(index) {
        $select = $(`<select class="form-control" id="${_id}-filter-logic-${index}"></select>`);
        for (let p in Constant.values.logic) {
            $select.append(`<option value="${p}">${Constant.values.logic[p]}</option>`)
        }

        return $(`<div class="col-md-2">`).append($select);
        //return $select;
    }
    function _createActionFilter(index) {
        $newBtn = $(`<button id="${_id}-filter-new-${index}" class="btn btn-default" style="border-radius:50%; margin-right:3px;"><i class="fa fa-plus"></i></button>`);
        $removeBtn = $(`<button id="${_id}-filter-remove-${index}" class="btn btn-danger" style="border-radius:50%"><i class="fa fa-times"></i></button>`);
        $col = $(`<div class="col-md-1">`);
        $col.append($newBtn);
        if (index > 0)
            $col.append($removeBtn);

        return $col;
    }

    return {
        CreateModalFilter: function (id, columns) {
            _createModal(id, columns)
        }
    }
})();




/*
{
    table:{
        id:'',
        columns:[],
        pageSize:10,
    },
    toolbarId:''
} 
 */

var DataGrid = (function (filter) {
    var _filter = filter;
    let _options = {
        icons: {
            columns: 'fa-th-list text-orange-d1',
            detailOpen: 'fa-plus text-blue',
            detailClose: 'fa-minus text-blue',
            export: 'fa-download text-blue',
            print: 'fa-print text-purple-d1',
            fullscreen: 'fa fa-expand',

            search: 'fa-search text-blue'
        },
        serverSort: true,
        sortable: true,
        sidePagination: 'server',
        pagination: 'true',
        //toolbar: "#table-toolbar",
        theadClasses: "bgc-white text-grey text-uppercase text-80",
        //clickToSelect: true,
        pageSize: 10,
        checkboxHeader: true,
        search: true,
        searchAlign: "left",
        //showSearchButton: true,

        sortable: true,

        //detailView: true,
        //detailFormatter: "detailFormatter",

        pagination: true,
        paginationLoop: false,

        buttonsClass: "outline-default bgc-white btn-h-light-primary btn-a-outline-primary py-1 px-25 text-95",

        showExport: true,
        showPrint: true,
        showColumns: true,
        showFullscreen: true,


        mobileResponsive: true,
        //checkOnInit: true,
        buttons: function () {
            return {
                btnFilter: {
                    //text: 'Highlight Users',
                    icon: 'fa-filter',
                    event: function () {
                        //alert('Do some stuff to e.g. search all users which has logged in the last week')
                        //$('#exampleModal').modal('show');
                        _filter.CreateModalFilter(_options.id, _options.columns);
                    }
                    //attributes: {
                    //    title: 'Search all users which has logged in the last week'
                    //}
                },
            }
        }
    }
    function DataGrid(options) {
        _initOptions(options);
        _createToolbar();
        //_filter.CreateModalFilter(_options.id, _options.columns);
        return _drawTable();
    }
    function _initOptions(options) {
        options.columns.unshift(_addCheckboxColumn());
        options.columns.push(_addDefaultActionColumn());
        var toolbarId = `${options.id}-table-toolbar`;
        options.toolbar = `#${toolbarId}`;
        _options = $.extend(true, _options, options);
    }
    function _addDefaultActionColumn(options) {
        return {
            field: 'tools',
            title: '<i class="fa fa-cog text-secondary-d1 text-130"></i>',
            width: 140,
            align: 'center',
            printIgnore: true,
            formatter: function (value, row, index, field) {
                return '<div class="action-buttons">\
                            <button class="btn btn-primary mx-1" >\
                              <i class="fa fa-pencil-alt text-105"></i>\
                            </button>\
                            <button class="btn btn-danger mx-1">\
                              <i class="fa fa-trash-alt text-105"></i>\
                            </button>\
                        </div>'
            }
        }
    }
    function _addCheckboxColumn() {
        return {
            field: 'state',
            checkbox: true,
            printIgnore: true,
            //width: 64
        }
    }
    function _createToolbar() {
        var html = `<button id="${_options.id}-new-btn" class="btn btn-xs p-2 mr-2 bgc-white btn-lighter-red radius-3px">
                        <i class="fa fa-plus text-125 mx-2px"></i>
                    </button>
                     <button disabled id="${_options.id}-remove-btn" class="btn btn-xs p-2 mr-2 bgc-white btn-lighter-red radius-3px">
                        <i class="fa fa-trash-alt text-125 mx-2px"></i>
                    </button>`;
        var $toolbar = $(`<div id=${_options.id}-table-toolbar>`);
        $toolbar.append(html);
        $(`#${_options.id}`).closest('div.card-body').prepend($toolbar);
    }
    function _tableEvents($table) {
        $table.on('check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table', function (data, status, jqXHR) {
            $(`#${_options.id}-remove-btn`).prop('disabled', !$table.bootstrapTable('getSelections').length)
        });
    }

    function _drawTable() {
        var $selector = $(`#${_options.id}`);
        var $table = $selector.bootstrapTable(_options);
        _tableEvents($table);
        return $table
    }

    return DataGrid;
})(FilterModal || {});


