$(() => {
    //alert('foo');
    const imageId = $('.imageId').val();

    setInterval(() => {        
        $.post("/home/getLikes", { Id: imageId }, function (likesCount) {
            $('.likeAmount').val(likesCount);
            console.log('set')
        });
    }, 1000)

    $('.likeBtn').on('click', function () {
        const imageId = $(this).data('id');
        $.post("/home/addLike", { Id: imageId }, function (message) {
            alert(message)

        })
    })
    

});