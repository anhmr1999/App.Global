$(function () {
    var l = abp.localization.getResource('Global');
    var index = 0;
    var viewModal = new abp.ModalManager(abp.appPath + 'Mails/EmailTemplateModal');
    let _editor;

    DecoupledEditor
        .create(document.querySelector('#defaultTemplate'))
        .then(editor => {
            const toolbarContainer = document.querySelector('#toolbar-container');
            _editor = editor;
            toolbarContainer.appendChild(editor.ui.view.toolbar.element);
        })
        .catch(error => {
            console.error(error);
        });

    var dataTable = $('#EmailTemplate').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: false,
            order: [],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(app.global.appServices.emails.emailTemplate.getList, function () {
                return { filter: $('.filter').val() };
            }),
            columnDefs: [
                {
                    title: l('index'),
                    data: null,
                    searchable: false,
                    sortable: false,
                    render: function () {
                        return ++index;
                    }
                },
                {
                    title: l('name'),
                    data: "templateName",
                    searchable: false,
                    sortable: false
                },
                {
                    title: l('title'),
                    data: "defaultTitle",
                    searchable: false,
                    sortable: false
                },
                {
                    title: l('allowUsing'),
                    data: "isActive",
                    searchable: false,
                    sortable: false,
                    render: function (isActive) {
                        if (isActive) {
                            return `<span class="btn btn-status btn-success" disable>can use</span>`;
                        }
                        return `<span class="btn btn-status btn-danger" disable>can't use</span>`;
                    }
                },
                {
                    title: l('allowChange'),
                    data: "allowChange",
                    searchable: false,
                    sortable: false,
                    render: function (allowChange) {
                        if (allowChange) {
                            return `<span class="btn btn-status btn-success" disable>can change</span>`;
                        }
                        return `<span class="btn btn-status btn-danger" disable>can't change</span>`;
                    }
                },
                {
                    title: l('action'),
                    data: null,
                    searchable: false,
                    sortable: false,
                    render: function (data) {
                        var result = `<button data-id="` + data.id + `" class="btn btn-outline-primary btn-sm view-template"><i class="fa fa-eye"></i></button>`;
                        if (abp.auth.isGranted("EmailManager.Template.Edit"))
                            result += `<button data-id="` + data.id + `" class="btn btn-outline-success btn-sm change-template"><i class="fa fa-wrench"></i></button>`;
                        if (abp.auth.isGranted("EmailManager.Template.Delete"))
                            result += `<button data-id="` + data.id + `" class="btn btn-outline-danger btn-sm delete-template"><i class="fa fa-times"></i></button>`;
                        return result;
                    }
                }
            ]
        })
    );

    function reloadTable() {
        index = 0;
        dataTable.ajax.reload();
    };
    async function showForm() {
        $('.CoUbackgroup').css('display', 'unset');
        await new Promise(resolve => setTimeout(resolve, 50));
        $('.CoUbackgroup .template-form').css('right', '0');
    }
    async function hideForm() {
        $('.CoUbackgroup .template-form').css('right', '-50%');
        await new Promise(resolve => setTimeout(resolve, 1000));
        $('.CoUbackgroup').css('display', 'none');

        $('input[name=id]').val('');
        $('input[name=templateName]').val('');
        $('input[name=defaultTitle]').val('');
        $('input[name=isActive]').prop('checked', true);
        $('input[name=allowChange]').prop('checked', true);
        _editor.setData('');
        $.each($('#params').find('div.param'), function (i, item) {
            $(this).remove();
        });
    }
    function getExtraProperties(template) {
        var data = {};
        var success = true;
        $.each($('#params').find('input.param'), function (i, item) {
            data[$(item).val()] = '';
            if (!template.includes($(item).val())) {
                abp.notify.warn("Param " + $(item).val() + ' can\'t use in template!');
                success = false;
            }
        });
        return { Success: success, data: data };
    }

    $('#createButton').click(function (e) {
        e.preventDefault();
        $('#form-label-header').html("create template");
        showForm()
    });
    $('#template-form-save').on('click', function (e) {
        e.preventDefault();
        abp.ui.setBusy();
        var template = {};
        template.id = $('input[name=id]').val();
        template.templateName = $('input[name=templateName]').val();
        template.defaultTitle = $('input[name=defaultTitle]').val();
        template.isActive = $('input[name=isActive]').is(":checked");
        template.allowChange = $('input[name=allowChange]').is(":checked");
        template.defaultTemplate = _editor.getData();
        var extraProps = getExtraProperties(template.defaultTemplate);
        debugger;
        if (extraProps.Success) {
            template.extraProperties = extraProps.data; if (template.id == '') {
                template.id = null;
                app.global.appServices.emails.emailTemplate.create(template).done(function (res) {
                    reloadTable();
                    abp.notify.success('Create new template successfully!');
                }).always(function () {
                    hideForm();
                    abp.ui.clearBusy();
                });
            }
            else
                app.global.appServices.emails.emailTemplate.update(template).done(function (res) {
                    reloadTable();
                    abp.notify.success('Template update successful!');
                }).always(function () {
                    hideForm();
                    abp.ui.clearBusy();
                });
        } else {
            abp.ui.clearBusy();
        }
    });
    $('#template-form-close').on('click', function (e) {
        hideForm();
    });
    $(document).on('click', '.change-template', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        $('#form-label-header').html("edit template");
        abp.ui.setBusy();
        app.global.appServices.emails.emailTemplate.get(id).done(function (res) {
            $('input[name=id]').val(id);
            $('input[name=templateName]').val(res.templateName);
            $('input[name=defaultTitle]').val(res.defaultTitle);
            $('input[name=isActive]').prop('checked', res.isActive);
            $('input[name=allowChange]').prop('checked', res.allowChange);
            _editor.setData(res.defaultTemplate);
            $.each(res.extraProperties, function (index, value) {
                $('#params').append('<div class="col-md-3 param"><input type="text" value="'
                    + index + '" class="param form-control" placeholder="Param"/><i class="fa fa-minus remove-param"></i></div>');
            });
            showForm();
        }).always(function () {
            abp.ui.clearBusy();
        });
    });
    $(document).on('click', '.delete-template', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        abp.message.confirm('Are you sure to delete this template?')
            .then(function (confirmed) {
                if (confirmed) {
                    abp.ui.setBusy();
                    app.global.appServices.emails.emailTemplate.delete(id)
                        .done(function (res) {
                            abp.notify.success('Delete Template Successfully!');
                            reloadTable();
                        })
                        .always(function () { abp.ui.clearBusy(); });
                }
            });
    });
    $(document).on('click', '.view-template', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        viewModal.open({ id: id });
    });
    $('.filter').on('change', function () {
        reloadTable();
    });
    $(document).on('click', '.addParam', function (e) {
        e.preventDefault();
        $('#params').append('<div class="col-md-3 param"><input type="text" class="param form-control" placeholder="Param"/><i class="fa fa-minus remove-param"></i></div>');
    });
    $(document).on('change', 'input.param', function () {
        var val = $(this).val().replace(new RegExp('{', 'g'), "").replace(new RegExp('}', 'g'), "");
        if (val != '')
            $(this).val('{{' + val + '}}');
        else
            $(this).val('');
    });
    $(document).on('click', '.remove-param', function () {
        $(this).parent().remove();
    });
});