document.addEventListener("DOMContentLoaded", function() {

    document.querySelectorAll("a").forEach(function (elem) {
        let href_element = elem.href;
        let text_obj = elem.querySelector("description");
        if (text_obj === null){
            return;
        }
        let text= "";
        if ( href_element.includes("vk_field")){
            text = "ВКонтакте: ";
        }
        if ( href_element.includes("tg_field")){
            text = "Телеграм: ";
        }
        if ( href_element.includes("old_base_field")){
            text = "Старые мастерские: ";
        }
        if ( href_element.includes("cnews_field")){
            text = "CNews: ";
        }
        if ( href_element.includes("academcity_field")){
            text = "Академсити: ";
        }
        if ( href_element.includes("elementy_field")){
            text = "Elementy: ";
        }
        if ( href_element.includes("ershov_archive_field")){
            text = "Архив Ершова: ";
        }
            text_obj.textContent = text + text_obj.textContent;


    });
});