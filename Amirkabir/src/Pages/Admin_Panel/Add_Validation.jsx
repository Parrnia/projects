const Validation = (props) => {
    let errors = {};
    let isValid = true;
    let urlRegex = /(https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?\/[a-zA-Z0-9]{2,}|((https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?)|(https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}(\.[a-zA-Z0-9]{2,})?/g;

    //Title
    if (!props.title) {
        errors.title = 'فیلد الزامی ';
        isValid = false;
    }
    
    //Video Link
    if (!props.videoLink) {
        errors.videoLink = 'فیلد الزامی ';
        isValid = false;
    }
    else if (!urlRegex.test(props.videoLink)) {
        errors.videoLink = 'لینک نامعتبر است';
        isValid = false;
    }
 
    return [errors, isValid];
}

export default Validation