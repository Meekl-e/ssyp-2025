document.addEventListener("DOMContentLoaded", function () {
    let button = document.getElementById("theme");
    let li = document.getElementsByTagName('li');
    let dark_theme = true;
    let button2 = document.getElementById('cs2');
    button.addEventListener("click", function() {

        if(dark_theme === true) {
            document.body.style.color = 'white'
            document.body.style.backgroundColor = 'black'
            dark_theme = false;
        } else {
            dark_theme = true;
            document.body.style.color = 'black'
            document.body.style.backgroundColor = '#ffffd8'

        }
    });


});

