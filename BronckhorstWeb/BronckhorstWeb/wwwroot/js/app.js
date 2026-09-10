document.addEventListener('alpine:init', () => {
    Alpine.data('products', () => ({
        items: [
            { id: 1, name: 'Implantaten',img: 'implantaten-1.jpg',price: 100.00,originalPrice: 150.00},
           
            { id: 2, name: 'Noxi', img: 'noxi-1.jpg', price: 200.00, originalPrice: 250.00 }
               ]
    }))
})