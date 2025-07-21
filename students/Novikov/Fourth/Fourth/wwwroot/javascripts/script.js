document.addEventListener('DOMContentLoaded', function () {
    const myCheckbox = document.getElementById('theme');
    let a = 'False'
    myCheckbox.addEventListener("click", function (){
        if (a === 'False') {
            document.body.style.color = 'white';
            document.body.style.background = 'black';
            let a = 'True'
        }

    })

})
