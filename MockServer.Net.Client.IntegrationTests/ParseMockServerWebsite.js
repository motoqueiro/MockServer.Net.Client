var elements = document.querySelectorAll("div.panel.title > button");
for (var i = 0; i < elements.length; i++) {
    elements[i].textContent;
}

var requestBodyElements = document.querySelectorAll("div.panel.title > div.panel > div.panel:nth-child(2) > pre > code > span.str:nth-child(1)");