
var swal = Swal.mixin({
    //buttonsStyling: false
    showConfirmButton:false
});
function formatDateTime(locale, value) {
    let dateTimeValue = new Date(value);
    let timeZone = dateTimeValue.getTimezoneOffset() * 60 * 1000;
    let localTimezone = dateTimeValue.getTime() - timeZone;
    return new Date(localTimezone).toLocaleDateString(locale) + " " + new Date(localTimezone).toLocaleTimeString(locale, { hour12: true });

}
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
            'and': 'And',
            'or': 'Or'
        }
    }
}

//create Modal when clicking filter button for user can create filter item on table
var FilterModal = (function () {
    function _createModal(id, columns, selector, $table) {
        this._controlName = {
            c: 'column',
            o: 'operator',
            v: 'value',
            l: 'logic',
            n: 'new',
            r: 'remove'
        };        

        this._id = id;
        this._columns = columns;
        this._table = $table;
        this._modalSelector = selector;

        //filter control
        this.modalId = `${this._id}-filter-modal`;
        this.body = '';
        this.saveId = `${this._id}-filter-save`;

        this._defaultText = {
            c: 'Please select column',
            o: 'Please select operator',
        }

        this.cookieFilterKey = `${this._id}-filter`;

        _createTemplate();
        $(`#${this.modalId}`).modal('show');
    }
    function _getSource(path, sourceKey) {
        return eval(path)[sourceKey];
    }
    function _createControlId(controlName, index) {

        return `${this._id}-filter-${this._controlName[controlName]}-${index}`;
    }
    function _createTemplate() {
        var html = `<div class="modal modal-xl fade" id="${this.modalId}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                      <div class="modal-dialog" role="document">
                        <div class="modal-content">
                          <div class="modal-header">
                            <h5 class="modal-title text-primary-d3">
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

                            <button type="button" id="${this.saveId}" class="btn btn-primary">
                              Save changes
                            </button>
                          </div>
                        </div>
                      </div>
                    </div>`;
        $(`#${this._modalSelector}`).empty();
        $(`#${this._modalSelector}`).append(html);
        this.body = $(`#${this.modalId} .modal-body`);


        let filters = _getFilterRestore();
        (filters) ? _restoreFilterBlock(filters) : _createFilterBlock(0);

        $(`#${this.saveId}`).on('click', function () {
            _saveFilter();
        });
    }
    function _restoreFilterBlock(filters) {
        for (let i = 0; i < filters.length; i++)
        {
            let filterObj = filters[i];
            _createFilterBlock(i, filterObj);
        }
        
    }
    function _getFilterRestore() {
        let value = sessionStorage.getItem(this.cookieFilterKey);
        if (value)
            value = JSON.parse(value);

        return value;
    }
    function _populateDropdownList(control, source, defaultValue) {
        control.empty();
        if (defaultValue)
            control.append(`<option>${defaultValue}</option>`);
        if (source == null)
            return;
        //columns source
        if (Array.isArray(source)) {
            for (let i = 0; i < source.length; i++) {
                let col = this._columns[i];
                let type = col.type;
                let field = col.field;
                let title = col.title;
                if (field != 'state' && field != 'tools')
                    control.append(`<option value="${field}_${type}">${title}</option>`);
            }
        }
        else {
            for (let p in source) {
                let val = p;
                let text = source[p];
                control.append(`<option value="${val}">${text}</options>`);
            }
        }
    }
    function _createFilterColumnControl($formRow, index, filterObj) {
        let id = _createControlId('c', index);
        let $col = $(`<div class="col-md-3">`);
        let $select = $(`<select class="form-control" id="${id}">`);
        _populateDropdownList($select, this._columns, this._defaultText.c);
        $col.append($select);
        $($formRow).append($col);

        //set selected column,
        if (filterObj) {
            $val = filterObj.field + '_' + filterObj.type;
            $select.val($val);
        }
    }
    function _createFilterOperatorControl($formRow, index, filterObj) {
        let id = _createControlId('o', index);
        let $col = $(`<div class="col-md-3">`);
        let $select = $(`<select class="form-control" id="${id}">`);
        _populateDropdownList($select, null, this._defaultText.o);
        $col.append($select);
        $($formRow).append($col);

        //set selected column,
        if (filterObj) {
            //get column control
            let $columnFilterControl = $(`#${_createControlId('c', index)}`);
            let columnSelectedValueType = $columnFilterControl.val().split('_')[1];
            let operatorSelectedValue = filterObj.operand;

             _populateDropdownList($select, _getSource('Constant.operators', columnSelectedValueType));
            $select.val(operatorSelectedValue);
        }
    }
    function _createFilterValueControl($fromRow, index, filterObj) {
        let id = _createControlId('v', index);
        let $col = $(`<div class="col-md-3">`);
        let $input = $(`<input class="form-control" id=${id} type="text">`);

        if (filterObj) {
            //restore
            //get column control
            let $columnFilterControl = $(`#${_createControlId('c', index)}`);
            let columnSelectedValueType = $columnFilterControl.val().split('_')[1];
            if (columnSelectedValueType == 'boolean') {
                $select = $(`<select class="form-control" id="${id}">`);
                _populateDropdownList($select, _getSource('Constant.values', 'boolean'))
                $select.val(filterObj.value);
                $input = $select;
            }
            else {
                $input.val(filterObj.value);
            }
        }
        $col.append($input);
        $fromRow.append($col);
    }
    function _createLogicControl($formRow, index, filterObj) {
        let id = _createControlId('l', index);
        let $col = $(`<div class="col-md-2">`);
        let $select = $(`<select class="form-control" id="${id}">`);

        let source = _getSource('Constant.values', 'logic');
        _populateDropdownList($select, source);
        if (filterObj) {
            $select.val(filterObj.logic);
        }
        $col.append($select);
        $formRow.append($col);
    }
    function _createActionButtonControl($formRow, index) {
        let btnNewId = _createControlId('n', index);
        let btnRemoveId = _createControlId('r', index);
        $col = $(`<div class="col-md-1">`);

        $newBtn = $(`<button id="${btnNewId}" class="btn btn-default" style="border-radius: 50%; margin-right: 3px; "><i class="fa fa-plus"></i></button>`);
        $removeBtn = $(`<button id="${btnRemoveId}" class="btn btn-danger" style="border-radius:50%"><i class="fa fa-times"></i></button>`);
        $col.append($newBtn);
        if (index > 0)
            $col.append($removeBtn);

        $formRow.append($col);
    }

    function _createFilterBlock(index, filterObj) {
        var styleCss = index > 0 ? 'style="margin-top:1rem";' : ''
        var $row = $(`<div class="form-row" ${styleCss}">`);
        this.body.append($row);

        _createFilterColumnControl($row, index, filterObj);
        _createFilterOperatorControl($row, index, filterObj);
        _createFilterValueControl($row, index, filterObj);
        _createLogicControl($row, index, filterObj);
        _createActionButtonControl($row, index, filterObj);

        _setListener(index);
    }

    function _setListener(index) {
        let columnFilterControlId = _createControlId('c', index);
        let operatorFilterControlId = _createControlId('o', index);
        let valueFilterControlId = _createControlId('v', index);
        let buttonNewFilterControlId = _createControlId('n', index);
        let buttonRemoveFilterControlId = _createControlId('r', index);

        let defaultText = this._defaultText;


        $(`#${columnFilterControlId}`).off('change');
        $(`#${columnFilterControlId}`).on('change', function () {
            let columnValue = $(this).val();
            let type = columnValue.split('_')[1];
            let $valueFilterControl = $(`#${valueFilterControlId}`);
            let $operatorFilterControl = $(`#${operatorFilterControlId}`);

            _populateDropdownList($operatorFilterControl, _getSource('Constant.operators', type), defaultText.o);

            if (type == 'boolean') {
                let $select = $(`<select class="form-control" id="${valueFilterControlId}">`);
                _populateDropdownList($select, _getSource('Constant.values', 'boolean'));
                $valueFilterControl.replaceWith($select);
            }
            else {
                let $input = $(`<input type="text" class="form-control" id="${valueFilterControlId}">`);
                $valueFilterControl.replaceWith($input);
            }
        });

        $(`#${operatorFilterControlId}`).off('change');
        $(`#${operatorFilterControlId}`).on('change', function () {
            let value = $(this).val();
            let display = 'block'
            if (value == 'isNull' || value == 'isNotNull') {
                display = 'none';
            }
            $(`#${valueFilterControlId}`).parent().css('display', `${display}`);
        });

        $(`#${buttonNewFilterControlId}`).off('click');
        $(`#${buttonNewFilterControlId}`).on('click', function () {

            _createFilterBlock((index + 1));
        });

        $(`#${buttonRemoveFilterControlId}`).off('click');
        $(`#${buttonRemoveFilterControlId}`).on('click', function () {

            $(`#${buttonRemoveFilterControlId}`).closest('.form-row').remove();
        });
    }

    function _saveFilter() {
        let result = [];
        let count = this.body.children().length;
        for (let i = 0; i < count; i++) {
            let $columnControlId = $(`#${_createControlId('c', i)}`);
            let $operatorControlId = $(`#${_createControlId('o', i)}`);
            let $valueControlId = $(`#${_createControlId('v', i)}`);
            let $logicControlId = $(`#${_createControlId('l', i)}`);

            if ($columnControlId.val() && $operatorControlId.val()) {
                result.push({
                    field: $columnControlId.val().split('_')[0],
                    type: $columnControlId.val().split('_')[1],
                    operand: $operatorControlId.val(),
                    value: $valueControlId.val(),
                    logic: $logicControlId.val()
                });
            }        
        }
        if (result.length > 0)
            sessionStorage.setItem(this.cookieFilterKey, JSON.stringify(result));

        $(`#${this.modalId}`).modal('hide');
        $(this._table).bootstrapTable('refresh');
    }

    return {
        createModalFilter: function (id, columns, selector, $table) {
            _createModal(id, columns, selector, $table);
        }
    }
})();

var ModalHelper = (function () {
    function _createModal(id, selector, url, title, callback) {
        this._url = url;
        this._id = id,//$table.attr('id');
        this._selector = selector;
        //this._$table = $table;
        this._title = title;
        this._callback = callback;

        this._modalId = `${this._id}-form-modal`;
        this._saveId = `${this._id}-form-save`;

        createTemplate();
        registerEvents(this);
        loadContent(this._url);
    }
    function createTemplate() {
        var html = `<div class="modal modal-xl fade" id="${this._modalId}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                      <div class="modal-dialog" role="document">
                        <div class="modal-content">
                          <div class="modal-header">
                            <h5 class="modal-title text-primary-d3">
                              ${this._title}
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

                            <button type="submit" id="${this._saveId}" class="btn btn-primary">
                              Save changes
                            </button>
                          </div>
                        </div>
                      </div>
                    </div>`;
        $(`#${this._selector}`).empty();
        $(`#${this._selector}`).append(html);        
    }
    function loadContent(url) {
        $(`#${this._modalId} .modal-title`).find('h5').html(this._title);
        $(`#${this._modalId} .modal-body`).load(url, function () {
            let modal = $(this).closest('div.modal-xl').modal('show');
           
        });
        //$(`#${this._modalId}`).modal('show');
    }

    function registerEvents(that) {
        let $table = that._$table;
        let $btnSave = $(`#${that._saveId}`);
        let modalId = that._modalId
        let callback = that._callback;

        $(`#${this._modalId}`).on('show.bs.modal', function () {
            let $form = $(this).find('form');
            let formId = $form.attr('id');
            let isNew = $form.attr('isNew');
            $btnSave.attr('form', formId);

            //$(`#${this} .modal-footer`).find('button[type=submit]').attr('form', formId);
            $form.submit(function () {
                return ajaxSubmit(this, modalId, isNew);
            })
        })

        $(`#${this._modalId}`).on('hide.bs.modal', function () {

            //if ajax call success: it will set data-isSubmit = true in modal attribute
            let isSubmit = $(this).data('isSubmit');
            if (isSubmit) {
                //self._closeCallback(this);
                //$table.bootstrapTable('refresh');
                callback();
                $(this).removeData('isSubmit');
            }
        });
    }
    return {
        createModal: function (tableId, selector, url, title, callback) {
            _createModal(tableId, selector, url, title, callback)
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

function DataTable(options) {
    this._defaultOptions = {
        url: options.urls.load,
        icons: {
            columns: 'fa-th-list text-orange-d1',
            detailOpen: 'fa-plus text-blue',
            detailClose: 'fa-minus text-blue',
            export: 'fa-download text-blue',
            print: 'fa-print text-purple-d1',
            fullscreen: 'fa fa-expand',

            sort: 'fas fa-sort',
            plus: 'fas fa-plus',
            minus: 'fas fa-minus',

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
        search: false,
        searchAlign: "left",
        smartDisplay: false,
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
                        let tableId = this.options.id;
                        let cols = this.options.columns[0];
                        let modalSelector = this.options.modalSelector;
                        FilterModal.createModalFilter(tableId, cols, modalSelector, this.$el);

                        //FilterModal.createModalFilter(_options.id, _options.columns, _options.modalSelector, _$table);
                    }
                    //attributes: {
                    //    title: 'Search all users which has logged in the last week'
                    //}
                },
            }
        }
        //queryParams: ()=>function (params) {
        //    debugger;
        //    params.search = sessionStorage.get(`${this._options.id}-filterExpression`);
        //    return params;
        //}
    }
    this._options = options;
    this._$table;
}
DataTable.prototype = (function () {
    function showFormModal($table, formSelector, url, title) {
        debugger;
        alert('aaaaa');
        //ModalHelper.createModal($table, formSelector, url, title);
    }
    function initOptions() {
        this._options.columns.unshift(_addCheckboxColumn());
        this._options.columns.push(_addDefaultActionColumn(this));
        var toolbarId = `${this._options.id}-table-toolbar`;
        this._options.toolbar = `#${toolbarId}`;
        this._options = $.extend(true, this._defaultOptions, this._options);
    }
    function _addDefaultActionColumn(that) {
        return {
            field: 'tools',
            title: '<i class="fa fa-cog text-secondary-d1 text-130"></i>',
            width: 140,
            align: 'center',
            printIgnore: true,
            formatter: function (value, row, index, field) {
                //test();
                return `<div class="action-buttons">\
                            <button class="btn btn-primary mx-1" onclick="ajaxEditItem('${that._options.id}', '${row.id}', '${row.title}');" >\
                              <i class="fa fa-pencil-alt text-105"></i>\
                            </button>\
                            <button class="btn btn-danger mx-1" onclick="ajaxDeleteItem('${that._options.id}','${row.id}', '${row.title}');">\
                              <i class="fa fa-trash-alt text-105"></i>\
                            </button>\
                        </div>`
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
    function init() {
        initOptions.call(this)
        createToolbar.call(this);
        drawTable.call(this);
        tableEvents.call(this);
    }
    function createToolbar() {
        var html = `<button id="${this._options.id}-new-btn" class="btn btn-xs p-2 mr-2 bgc-white btn-lighter-red radius-3px">
                        <i class="fa fa-plus text-125 mx-2px"></i>
                    </button>
                     <button disabled id="${this._options.id}-remove-btn" class="btn btn-xs p-2 mr-2 bgc-white btn-lighter-red radius-3px">
                        <i class="fa fa-trash-alt text-125 mx-2px"></i>
                    </button>`;
        var $toolbar = $(`<div id=${this._options.id}-table-toolbar>`);
        $toolbar.append(html);
        $(`#${this._options.id}`).closest('div.card-body').prepend($toolbar);
    }
    function tableEvents() {
        let id = this._options.id;
        let formSelector = this._options.formSelector;
        let $table = this._$table;
        let newUrl = this._options.urls.create;
        let deleteUrl = this._options.urls.delete;

        $table.on('check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table', function (data, status, jqXHR) {
            $(`#${id}-remove-btn`).prop('disabled', !$table.bootstrapTable('getSelections').length)
        });

        //regis event for delete(all) and new button 
        var tableId = this._options.id;
        $(`#${tableId}-new-btn`).off('click');
        $(`#${tableId}-new-btn`).on('click', function () {
            ModalHelper.createModal(tableId, formSelector, newUrl, 'Create new category', function () {
                $table.bootstrapTable('refresh');
            });
        })

        $(`#${tableId}-remove-btn`).off('click');
        $(`#${tableId}-remove-btn`).on('click', function () {
            let selectedItems = $table.bootstrapTable('getSelections');
            let count = selectedItems.length;
            let ids = '';
            for (let i = 0; i < count; i++) {
                ids += `${selectedItems[i].id},`;
            }
            let last = ids.lastIndexOf(',');
            ids = ids.substring(0, last);
            ajaxDeleteItem(id, ids, '', deleteUrl);
            //ModalHelper.createModal($table, formSelector, deleteUrl);
        })
    }

    function drawTable() {
        var $selector = $(`#${this._options.id}`);
        this._$table = $selector.bootstrapTable(this._options);
    }
    

    return {
        create: init
    }
})()

function ajaxSubmit(form, modal, isNew) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("RequestVerificationToken",
                    $('input:hidden[name="__RequestVerificationToken"]').val());

            },
            success: function (res) {
                if (res.isSuccess) {
                    $(`#${modal}`).data('isSubmit', true);
                    $(`#${modal} .modal-body`).html('');
                    $(`#${modal} .modal-title`).html('');
                    $(`#${modal}`).modal('hide');

                    swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: isNew === 'true' ? `${res.data.title} is inserted successfull` : `${res.data.title} is updated successfull`,
                        timer: 1000
                    })
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

var ajaxDelete = function (id, title, url, callback) {
    swal.fire({
        icon: 'question',
        title: title ? `Do you want to delete&nbsp;<b>${title}</b> ?` : 'Do you want to delete selected items ?',
        //text: `${res.message}`,
        showConfirmButton: true,
        showCancelButton: true,
        confirmButtonText: `Delete`,
        //timer: 1500
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `${url}?ids=[${id}]`,
                type: 'post',
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                beforeSend: function (xhr) {
                    //xhr.setRequestHeader('MY-XSRF-TOKEN', $('input:hidden[name="__RequestVerificationToken"]').val())
                    xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                }
            }).done(function (res) {
                if (res.isSuccess) {
                    swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: res.data ? `${res.data.title} is deleted successfully` : `Deleting is successfully`,
                        timer: 1500
                    });
                    //$table.draw();

                    callback();
                    return false;
                }
                else {
                    swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: `${res.message}`,
                        showConfirmButton: true
                        //timer: 1500
                    });
                    return false
                }
            }).fail(function (err) {
                alert('error');
            });
        }
    });
}


var ajaxEditItem = function (tableId, itemId, itemTitle) {
    let $table = $(`#${tableId}`);
    let option = $table.bootstrapTable('getOptions');
    ModalHelper.createModal(option.id, option.formSelector, option.urls.update + '/' + itemId, itemTitle, function () {
        $table.bootstrapTable('refresh');
    });
    //console.log(t);
}
var ajaxDeleteItem = function (tableId, itemIds, itemTitle, url) {
    let $table = $(`#${tableId}`);
    let option = $table.bootstrapTable('getOptions');

    ajaxDelete(itemIds, itemTitle, option.urls.delete, function () {
        let $table = $(`#${tableId}`);
        $table.bootstrapTable("refresh");
    })
}