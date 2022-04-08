import Vue from 'vue'

Vue.filter('shortenText', function(text, maxLength = 300) {
    if (text && typeof text === 'string') {
        const finalChar = text.length > maxLength ? '...' : ''
        return text.substr(0, maxLength) + finalChar
    }

    return ''
})

Vue.filter('currency', function(value) {
    var amount = '$' + value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')
    return amount
})