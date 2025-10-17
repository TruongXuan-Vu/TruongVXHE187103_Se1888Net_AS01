const MODAL_TYPES_CONST = {
    create: {
        name: "Create",
        value: 1,
        title: "Create A New Account",
        sumbmitText: "Create"
    },
    update: {
        name: "Update",
        value: 2,
        title: "Edit Account Information",
        sumbmitText: "Save"
    },
    changePassword: {
        name: "Change Password",
        value: 3,
        title: "Change Password",
        sumbmitText: "Change"
    },
    delete: {
        name: "Delete",
        value: 4,
        title: "Delete Account",
        sumbmitText: "OK"
    }
}

$(document).ready(function () {
    $('.modal').on('show.bs.modal', function (event) {
        const button = $(event.relatedTarget);
        const modalType = button.data('action-type');
        const accountId = button.data('id');
        const modal = Object.values(MODAL_TYPES_CONST).find(x => x.value == modalType);
        $('.modal-title').text(modal.title);

        const btnSubmit = $('.btn-submit');
        btnSubmit.text(modal.sumbmitText);
        if (modal.value == MODAL_TYPES_CONST.delete.value) {
            $('.btn-submit').addClass('btn-danger');
        }

        switch (modal.value) {
            case MODAL_TYPES_CONST.create.value:
                btnSubmit.attr({
                    'action-type': MODAL_TYPES_CONST.create.value,
                })
                break;
            case MODAL_TYPES_CONST.update.value:
                btnSubmit.attr({
                    'action-type': MODAL_TYPES_CONST.update.value,
                    'id': accountId
                })
                break;
            case MODAL_TYPES_CONST.changePassword.value:
                btnSubmit.attr({
                    'action-type': MODAL_TYPES_CONST.changePassword.value,
                    'id': accountId
                })
                break;
            case MODAL_TYPES_CONST.delete.value:
                btnSubmit.attr({
                    'action-type': MODAL_TYPES_CONST.delete.value,
                    'id': accountId
                })
                break;
        }


        $.ajax({
            url: '/Account/GetAccountForm',
            type: 'GET',
            data: { actionType: modalType, accountId: accountId },
            success: function (html) {
                $('#modal .modal-body').html(html);

                $.validator.unobtrusive.parse('#account-form');
            }
        });
    });

    $('#account-form').on('submit', function (e) {
        e.preventDefault();
        const modalType = $('.btn-submit').attr('action-type');
        const modal = Object.values(MODAL_TYPES_CONST).find(x => x.value == modalType);

        $('#action-type').val(modal.value);
        switch (modal.value) {
            case MODAL_TYPES_CONST.create.value:
                const isPassValid = validateNewConfirm('#password', '#confirm-password', '.err-pass-msg', '.err-confirm-msg');
                if (!$(this).valid() || !isPassValid) {
                    return;
                }

                $.ajax({
                    url: '/Account/CreateUpdate',
                    type: 'POST',
                    data: $(this).serialize(),
                    success: function (response, status, xhr) {
                        // Nếu server trả về JSON → lỗi hệ thống
                        if (xhr.getResponseHeader("Content-Type").includes("application/json")) {
                            toastr.error(response.message || "Unexpected error");
                            return;
                        }

                        // Nếu trả về partial table → cập nhật bảng
                        $('#account-table-container').html(response);
                        $('#modal').modal('hide');
                        toastr.success('Account saved successfully!');
                    },
                    error: function (xhr) {
                        // Nếu server trả về partial form (lỗi validation / email trùng)
                        if (xhr.status === 400) {
                            $('#modal .modal-body').html(xhr.responseText);
                            $.validator.unobtrusive.parse($('#account-form')); // parse lại validate
                        } else {
                            toastr.error('Something went wrong!');
                        }
                    }
                });

                break;
            case MODAL_TYPES_CONST.update.value:
                this.submit();
                break;
            case MODAL_TYPES_CONST.changePassword.value:
                if (!validateOldNew('#old-password', '#new-password', '.err-new-msg') ||
                    validateNewConfirm('#new-password', '#confirm-password', '#err-new-pass', '#err-confirm-pass')) {
                    return;
                }
                break;
            case MODAL_TYPES_CONST.delete.value:
                this.submit();
                break;
        }
    });
});

function validateOldNew(oldSelector, newSelector, errorNewSelector) {
    const oldPass = $(oldSelector).val().trim();
    const newPass = $(newSelector).val().trim();
    const $errorNew = $(errorNewSelector);

    $errorNew.text(''); // reset lỗi

    if (!newPass) {
        $errorNew.text('Password is required!');
        return false;
    }

    if (newPass.length < 2) {
        $errorNew.text('Password must be at least 2 characters long!');
        return false;
    }

    if (oldPass && oldPass === newPass) {
        $errorNew.text('New password must be different from old password!');
        return false;
    }

    return true;
}

function validateNewConfirm(newSelector, confirmSelector, errorNewSelector, errorConfirmSelector) {
    const newPass = $(newSelector).val().trim();
    const confirmPass = $(confirmSelector).val().trim();
    const $errorNew = $(errorNewSelector);
    const $errorConfirm = $(errorConfirmSelector);
    let isValid = true;

    $errorNew.text('');
    $errorConfirm.text('');
    if (!newPass) {
        $errorNew.text('Password is required!');
        isValid = false;
    } else if (newPass.length < 2) {
        $errorNew.text('Password must be at least 2 characters long!');
        isValid = false;
    }

    if (!confirmPass) {
        $errorConfirm.text('Password is required!');
        isValid = false;
    } else if (confirmPass.length < 2) {
        $errorConfirm.text('Password must be at least 2 characters long!');
        isValid = false;
    } else if (newPass !== confirmPass) {
        $errorConfirm.text('Confirm password does not match!');
        isValid = false;
    }

    if (!isValid) {
        return false;
    }
    return true;
}