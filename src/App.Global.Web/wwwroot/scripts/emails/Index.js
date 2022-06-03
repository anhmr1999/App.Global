$(function () {
    var l = abp.localization.getResource('Global');

    var configModal = new abp.ModalManager(abp.appPath + 'Mails/ConfigModal');
    var viewModal = new abp.ModalManager(abp.appPath + 'Mails/EmailModal');

    var dataTable = $('#Email-Service').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: false,
            order: [],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(app.global.appServices.emails.email.getList,
                function () {
                    var inputfilter = { filter: $('.filter').val() };
                    if ($('.cbxStatus').val() != '') {
                        inputfilter.status = $('.cbxStatus').val();
                    }
                    return inputfilter;
                }
            ),
            columnDefs: [
                {
                    title: l('typeEmail'),
                    data: "systemEmail",
                    searchable: false,
                    sortable: false,
                    render: function (systemEmail) {
                        if (systemEmail) {
                            return `<span class="status processing">System</span>`;
                        }
                        return ``;
                    }
                },
                {
                    title: l('receiver'),
                    data: "receiverEmail",
                    searchable: false,
                    sortable: false
                },
                {
                    title: l('status'),
                    data: "status",
                    searchable: false,
                    sortable: false,
                    render: function (status) {
                        return getStatus(status);
                    }
                },
                {
                    title: l('template'),
                    data: "templateDto",
                    searchable: false,
                    sortable: false,
                    render: function (templateDto) {
                        if (templateDto != null) {
                            return templateDto.templateName;
                        }
                        return "";
                    }
                },
                {
                    title: l('title'),
                    data: 'templateDto',
                    searchable: false,
                    sortable: false,
                    render: function (templateDto) {
                        if (templateDto != null) {
                            return templateDto.defaultTitle;
                        }
                        return 'Global App Email...';
                    }
                },
                {
                    title: l('action'),
                    data: null,
                    searchable: false,
                    sortable: false,
                    render: function (data) {
                        var html = `<button data-id="` + data.id + `" class="btn btn-outline-success btn-sm view"><i class="fa fa-eye"></i></button>`;
                        if (data.status == 3)
                            html += `<button data-id="` + data.id + `" class="btn btn-outline-primary btn-sm resend" style="margin-left: 10px; position: relative;">
                                <i class="fa fa-envelope"></i><span class="timeofsend">` + data.numberOfTimeSend + `</span></button>`;
                        else
                            html += `<button data-id="` + data.id + `" class="btn btn-outline-primary btn-sm" disabled style="margin-left: 10px; position: relative;">
                                <i class="fa fa-envelope"></i><span class="timeofsend">` + data.numberOfTimeSend + `</span></button>`;

                        return html;
                    }
                }
            ]
        })
    );

    function getStatus(status) {
        if (status == 0) {
            return `<span class="status created">` + l('created') + `</span>`;
        }
        if (status == 1) {
            return `<span class="status processing">` + l('processing') + `</span>`;
        }
        if (status == 2) {
            return `<span class="status done">` + l('done') + `</span>`;
        }
        if (status == 3) {
            return `<span class="status fail">` + l('fail') + `</span>`;
        }
        if (status == 4) {
            return `<span class="status reSended">` + l('reSended') + `</span>`;
        }
        return "";
    }

    $('#EmailConfig').on('click', function (e) {
        e.preventDefault();
        configModal.open();
    });
    $('.filter, .cbxStatus').on('change', function () {
        dataTable.ajax.reload();
    });
    $(document).on('click', '.resend', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        abp.message.confirm('Are you sure to resend this email?')
            .then(function (confirmed) {
                if (confirmed) {
                    abp.ui.setBusy();
                    app.global.appServices.emails.email.reSend(id)
                        .done(function (res) {
                            abp.notify.success('Resend email sucessfully!');
                            console.log(res);
                        })
                        .always(function () {
                            dataTable.ajax.reload();
                            abp.ui.clearBusy();
                        });
                }
            });
    });
    $(document).on('click', '.view', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        viewModal.open({ id: id });
    });
});