(() => {
    const parent = document.getElementById('ParentCategoryId');
    const child = document.getElementById('CategoryId');
    const categories = JSON.parse(document.getElementById('catalog-categories').textContent);

    parent.addEventListener('change', () => {
        child.replaceChildren(new Option(parent.value ? 'Alle subcategorieën' : 'Kies eerst een hoofdcategorie', ''));
        child.disabled = !parent.value;

        for (const category of categories.filter(category => String(category.ParentId) === parent.value)) {
            child.add(new Option(category.TranslatedName ?? category.Name, category.Id));
        }
    });
})();
