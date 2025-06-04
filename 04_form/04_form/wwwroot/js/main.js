
document.getElementById('avatar').addEventListener('change', function (e) {
    const preview = document.getElementById('preview');
    const file = e.target.files[0];
    if (file) {
        preview.hidden = false;
        preview.src = URL.createObjectURL(file);
    }
});
