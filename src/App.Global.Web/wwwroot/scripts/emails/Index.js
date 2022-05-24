$(function () {
    var l = abp.localization.getResource('Global');
    var index = 0;

    var configModal = new abp.ModalManager(abp.appPath + 'Mails/EmailConfigModal');

    var dataTable = $('#Email-Service').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: false,
            order: [],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(app.global.appServices.emails.email.getList,
                {}
            ),
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
                    title: l('systemEmail'),
                    data: "code",
                    searchable: false,
                    sortable: false,
                    render: function (code) {
                        if (code) {
                            return `<input type="checkbox" disabled checked/>`;
                        }
                        return `<input type="checkbox" disabled />`;
                    }
                },
                {
                    title: l('receiver'),
                    data: "description",
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
                    title: l('action'),
                    data: null,
                    searchable: false,
                    sortable: false,
                    render: function (data) {
                        return ``;
                    }
                }
            ]
        })
    );

    function getStatus(status) {
        if (status == 0) {
            return `<div class="status created">` + l('created') + `</div>`;
        }
        if (status == 1) {
            return `<div class="status processing">` + l('processing') + `</div>`;
        }
        if (status == 2) {
            return `<div class="status done">` + l('done') + `</div>`;
        }
        if (status == 3) {
            return `<div class="status fail">` + l('fail') + `</div>`;
        }
        if (status == 4) {
            return `<div class="status reSended">` + l('reSended') + `</div>`;
        }
        return "";
    }

    $('#EmailConfig').on('click', function (e) {
        e.preventDefault();
        configModal.open();
    });
});