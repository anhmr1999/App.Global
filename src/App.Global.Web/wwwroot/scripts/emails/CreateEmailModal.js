$(abp.modals.CreateEmailModal = function () {
    let _editor;
    var $modal;
    function initModal(modalManager, args) {
        $modal = modalManager.getModal();
        registerEvent();

        DecoupledEditor
            .create(document.querySelector('#content-change'))
            .then(editor => {
                const toolbarContainer = document.querySelector('#toolbar-container');
                _editor = editor;
                toolbarContainer.appendChild(editor.ui.view.toolbar.element);
            })
            .catch(error => {
                console.error(error);
            });
    };

    function registerEvent() {
        $('.templateCbx').on('change', function (e) {
            abp.ui.setBusy('body');
            $('.params-container').empty();
            var templateId = $(this).val();
            if (templateId != '')
                app.global.appServices.emails.emailTemplate.get(templateId)
                    .done(function (res) {
                        console.log(res);
                        $('.email-title').val(res.defaultTitle);
                        if (res.extraProperties != {}) {
                            $.each(res.extraProperties, function (index, value) {
                                $('.params-container').append('<div class="col-4"><input class="form-control" name="' + index + '" placeholder="Enter ' + index + '"/></div>');
                            });
                            $('.params-content').css('display', 'flex');
                        }
                        if (!res.allowChange) {
                            _editor.enableReadOnlyMode('#content-change');
                            _editor.enableReadOnlyMode('#toolbar-container');
                        }
                        _editor.setData(res.defaultTemplate);
                    })
                    .always(function () {
                        abp.ui.clearBusy('body');
                    });
            else {
                emptyEmail();
                $('.params-container').empty();
                $('.email-title').val('');
                $('.params-content').css('display', 'none');
                _editor.disableReadOnlyMode('#content-change');
                _editor.disableReadOnlyMode('#toolbar-container');
                _editor.setData('');
                abp.ui.clearBusy('body');
            }
        });

        $('#saveEmail').on('click', function (e) {
            var email = {};
            email.Title = $('.email-title').val();
            email.ReceiverEmail = $('.email-receiver').val();
            if ($('.templateCbx').val() != '') {
                email.Content = _editor.getData();
                email.ExtraProperties = getExtraProp();
                email.TemplateId = $('.templateCbx').val();
            } else {
                email.ExtraProperties = {};
                email.Content = _editor.getData();
            }
            abp.message.confirm("Are you sure you want to send this email?")
                .then(function (confilm) {
                    if (confilm) {
                        app.global.appServices.emails.email.create(email)
                            .done(function (res) {

                            })
                            .always(function () {
                                $modal.modal('hide');
                            });
                    }
                });
        });

        function getExtraProp() {
            var result = {};
            $.each($('.params-container').find('input'), function (index, item) {
                result[$(item).attr('name')] = $(item).val();
            });
            return result;
        }
    }

    return {
        initModal: initModal
    };
});