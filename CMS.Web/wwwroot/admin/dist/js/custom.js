//const { Modal } = require("bootstrap");
var Toast = Swal.mixin({
    //toast: true,
    //position: 'top-end',
    showConfirmButton: false
    //timer: 3000
});

function showPassword(iconId, inputName, lockClass, unclockClass) {
    $(`#${iconId}`).on('mouseup', function () {
        //$(this).attr('class', lockClass);
        $(this).children().attr('class', lockClass);
        $(`input[name="${inputName}"]`).attr('type', 'password');
    }).on('mousedown', function () {
        //$(this).attr('class', unclockClass);
        $(this).children().attr('class', unclockClass);
        $(`input[name="${inputName}"]`).attr('type', 'text');
    });
}

var ajaxDelete = function (id, title, url, callback) {
    Toast.fire({
        icon: 'question',
        title: title ? `Do you want to delete ${title} ?` : 'Do you want to delete selected items ?',
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
                    Toast.fire({
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
                    Toast.fire({
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

var ajaxFormAction = function (selector, modalId, title, url, isNew, callback) {
    new ModalHelper(selector, modalId, title, url, isNew, callback).show();
}

var ajaxSubmit = (form, modal, isNew) => {
    debugger;
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
                    $(`#${modal} .modal-body`).html('');
                    $(`#${modal} .modal-title`).html('');

                    $(`#${modal}`).data('isSubmit', true);
                    $(`#${modal}`).modal('hide');
                    Toast.fire({
                        icon: 'success',
                        title: 'Success',
                        text: isNew ? `${res.data.title} is inserted successfull` : `${res.data.title} is updated successfull`,
                        timer: 1500
                    });
                    return false
                }
                else {
                    Toast.fire({
                        icon: 'error',
                        title: 'Error',
                        text: `${res.message}`,
                        showConfirmButton:true
                        //timer: 1500
                    });
                    return false
                }
                $(`#${modal} .modal-body`).html(res.html);
            },
            error: function (err) {
                alert('Ajax Error AjaxSubmit');
                console.log(err);
            }
        });

        // remove submit event on form. Because it will be register again when opening modal
        $(form).off();

        //to prevent default form submit event
        return false;
    } catch (ex) {
        alert('Error AjaxSubmit: ' + ex);
        console.log(ex)
    }
}

var ModalHelper = (function () {
    var ModalHelper = function (selector, modalId, title, url, isNew, closeCallback) {
        this._selector = selector;
        this._modalId = modalId;
        this._title = title;
        this._url = url;
        this._closeCallback = closeCallback;
        this._isNew = isNew;

        buildUI(this);
        registerEvent(this);
    }
    function buildUI(self) {
        let html = `<div class="modal fade" id="${self._modalId}">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">${self._title}</h4>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">

                            </div>
                            <div class="modal-footer justify-content-between">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                <button type="submit" form="${self._form}" class="btn btn-primary">Save</button>
                            </div>
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->
                </div>`;

        $(`#${self._selector}`).empty();
        $(html).appendTo($(`#${self._selector}`));
    }
    function registerEvent(self) {
        $(`#${self._modalId}`).on('show.bs.modal', function (e) {
            $(`#${self._modalId} .modal-body`).load(self._url, function (responseText, textStatus, jqXHR) {
                setSubmitForm(self._modalId, self._isNew);
            });
        });
        $(`#${self._modalId}`).on('hide.bs.modal', function (e) {
            //if ajax call success: it will set data-isSubmit = true in modal attribute
            let isSubmit = $(this).data('isSubmit');
            if (isSubmit) {
                self._closeCallback(this);
                $(this).removeData('isSubmit');
            }
            
        });
    }
    function setSubmitForm(modalId, isNew) {
        let form = $(`#${modalId}`).find('form');
        let formId = form.attr('id');
        $(`#${modalId} .modal-footer`).find('button[type=submit]').attr('form', formId);

        $(`#${modalId}`).find('form').on('submit', function () {
            return ajaxSubmit(this, modalId, isNew);
        })
    }

    ModalHelper.prototype.show = function () {
        $(`#${this._modalId}`).modal('toggle');
    }
    return ModalHelper;
})()

/**
options = {
        modal: {
            id: '',
            title: '',
            form: '',
            selector: ''
        },
        table: {
            id: '',
            urls: {
                load: '',
                create: '',
                delete: ''
            },
            columns:[]
        }
    }
 */

var Table = (function (o) {
    let _options = {
        modal: {
            id: '',
            title: '',
            form: '',
            selector: ''
        },
        table: {
            id: '',
            urls: {
                load: '',
                create: '',
                delete: ''
            },
            columns:[]
        }
    };
    let Table = function (options) {
        _buildOptions(options);
        //_createModal();
        return _createTable();
    }
    let _buildOptions = function (opt) {
        _options = opt;
    }
    let _createTable = function () {
        let $table = $(`#${_options.table.id}`).DataTable({
            dom: 'Blfrtip',
            //searchBuilder: true,
            columnDefs: [{ type: "office", targets: 2 }],
            //search: {
            //    return: true
            //},
            searchBuilder: {
                columns: [2, 3],
                enterSearch: true
            },
            "processing": true, // for show progress bar
            "serverSide": true,
            "filter": true,
            "pageLength": 2,
            "pagingType": "full_numbers",
            "ajax": {
                "url": _options.table.urls.load,
                "type": "POST",
                "datatype": "json"
                //"data": {
                //    "title": 'aa'
                //}
            },
            "ordering": true,
            "order": [[2,'asc']],
            //"order": [[0, 'asc'], [1, 'asc']]
            "columns": _options.table.columns,
            "createdRow": function (row, data, dataIndex) {
                let checkbox = $(row).children('td').first().find('input[type="checkbox"]');
                let headerCheckbox = $($table.column().header()).find('input[type=checkbox]');


                headerCheckbox.prop('checked', false);
                checkbox.on('change', { rowIndex: dataIndex }, function (e) {
                    let checked = $(this).prop('checked');
                    let headerCheckbox = $($table.column().header()).find('input[type=checkbox]');

                    let tableId = $table.table().node().id;
                    let deletebutton = $(`button[aria-controls = ${tableId}][name=delete]`);

                    let rowIdx = e.data.rowIndex;
                    if (checked) {
                        $table.rows(rowIdx).select();
                        $count++;
                        if ($count == $table.rows().count()) {
                            headerCheckbox.prop('checked', true);
                        }
                        deletebutton.attr('disabled', false);
                    }
                    else {
                        $table.rows(rowIdx).deselect();
                        $count--;
                        if ($count == 0) {
                            deletebutton.attr('disabled', true);
                        }
                        headerCheckbox.prop('checked', false);

                    }
                });
            },
            "initComplete": function (settings, json) {
                // Add custom button into table wrapper
                $table.buttons().container().appendTo(`#${_options.table.id}_wrapper .col-md-6:eq(0)`);

                //Select all bycheckbox header
                let headerCheckbox = $($table.column().header()).find('input[type=checkbox]');
                headerCheckbox.on('change', { settings: settings }, function (e) {
                    let checked = $(this).prop('checked');
                    for (let i = 0; i < $table.rows().count(); i++) {
                        $($table.rows(i).column().nodes()[i]).find('input[type="checkbox"]').prop('checked', checked);
                    }
                    let deletebutton = $(`button[aria-controls = ${_options.table.id}][name=delete]`).attr('disabled', !checked);
                    (checked) ? $table.rows().select() : $table.rows().deselect();
                });
            },
            "responsive": true,
            "lengthChange": false,
            "autoWidth": false,
            language: {
                searchBuilder: {
                    button: {
                        0: 'Criteria',
                        1: 'Criteria (one selected)',
                        _: 'Criteria (%d)'
                    }
                }
            },
            "buttons": [
                {
                    text: `<i class="fas fa-plus"> New</i>`,
                    className: 'btn btn-primary form-group',
                    attr: {
                        'name': 'new',
                        'style': 'background: #007bff !important; border-color: #007bff !important'
                    },
                    action: function (e, dt, node, config) {
                        ajaxFormAction(_options.modal.selector, _options.modal.id, 'Create new category', '/Admin/Category/Create', true, function (e) {
                            $table.draw();
                        });
                        
                    }
                },
                {
                    text: `<i class="fas fa-trash"> Delete</i>`,
                    className: 'btn btn-danger form-group',
                    attr: {
                        'name': 'delete',
                        //"delete": true,
                        "disabled": true
                    },
                    action: function (e, dt, node, config) {
                        let ids = [];
                        let rows = $table.rows({ selected: true }).every(function (rowIdx, tableLoop, rowLoop) {
                            ids.push(this.data().id);
                        });
                        ids = ids.join(',');
                        ajaxDelete(ids, null, _options.table.urls.delete, function (e) {
                            $table.draw();
                        });
                    }
                },
                {
                    text: 'Filter',
                    className: 'btn btn-default form-group',
                    //className: 'dtsb-add dtsb-button',
                    extend: 'searchBuilder',
                    //attr: {
                    //    'name': 'delete',
                    //    //"delete": true,
                    //    "disabled": true
                    //},
                    //action: function (e, dt, node, config) {
                       
                    //}
                }
            ]            
        })
        $('<input id="example1_column" type="text" class="form-control form-control-sm" placeholder="aaaa" />').insertBefore($("#example1_filter input"));
        return $table
    }

    //let _createModal = function () {
    //    let html = `<div class="modal fade" id="${_options.modal.id}">
    //                            <div class="modal-dialog">
    //                                <div class="modal-content">
    //                                    <div class="modal-header">
    //                                        <h4 class="modal-title">${_options.modal.title}</h4>
    //                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
    //                                            <span aria-hidden="true">&times;</span>
    //                                        </button>
    //                                    </div>
    //                                    <div class="modal-body">

    //                                    </div>
    //                                    <div class="modal-footer justify-content-between">
    //                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
    //                                        <button type="submit" class="btn btn-primary">Save</button>
    //                                    </div>
    //                                </div>
    //                                <!-- /.modal-content -->
    //                            </div>
    //                            <!-- /.modal-dialog -->
    //                        </div>`;

    //    $(html).appendTo($(`#${_options.modal.selector}`));
    //    _registerCloseModal();
    //}
    //let _submitForm = function (isNew) {
    //    let form = $(`#${_options.modal.id} form`);
    //    $(`#${_options.modal.id}`).find('button[type=submit]').attr('form', form.attr('id'));
    //    form.on('submit', function () {
    //        return ajaxSubmit(this, _options.modal.id, isNew);
    //    });
    //}
    //let _registerCloseModal = function () {
    //    $(`#${_options.modal.id}`).on('hidden.bs.modal', function (e) {
    //        let isSubmit = $(this).data('isSubmit');
    //        if (isSubmit) {
    //            _options.modal.closeCallback(this);
    //            $(this).removeData('isSubmit');
    //        }
    //    });
    //}

    return Table;
})()


//$.fn.dataTableExt.oApi.fnAddDataAndDisplay = function (oSettings, aData) {
//    alert('aaa');
//}