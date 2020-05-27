var user = {
    init: function () {
        user.egisterEvents();
    },
    egisterEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = $(this).data('id');
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('kich hoat');
                    }
                    else {
                        btn.text('khoa');
                    }
                }

            });
        });
    }
}
user.init();