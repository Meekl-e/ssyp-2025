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

document.addEventListener("DOMContentLoaded", function() {

    document.getElementById("blurred_el").classList.add("loaded");
    document.getElementById("blurred_el1").classList.add("loaded");
    document.getElementById("blurred_el2").classList.add("loaded");
    document.getElementById("blurred_el3").classList.add("loaded");
});