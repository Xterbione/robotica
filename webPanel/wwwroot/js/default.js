//darkmode toggle
function toggle(){
    const element = document.getElementById("body");
    if(element.getAttribute('data-bs-theme') == "dark"){
        element.setAttribute('data-bs-theme', 'light');
    } else{
        element.setAttribute('data-bs-theme', 'dark');
    }
    
}