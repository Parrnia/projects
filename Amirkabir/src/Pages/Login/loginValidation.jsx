const Validation = (props) => {
    let errors = {};
    let isValid = true;
    let passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
    let emailRegex = /^[a-zA-Z0-9_.±]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$/;
    const phoneRegex = /^09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}$/;

    //EmailOrPhone
    if (!props.emailOrPhone) {
        errors.emailOrPhone = 'فیلد الزامی ';
        isValid = false;
    }
    else if (!emailRegex.test(props.emailOrPhone) && !phoneRegex.test(props.emailOrPhone)) {
        errors.emailOrPhone = 'ایمیل یا شماره تلفن نامعتبر است';
        isValid = false;
    }
    
    //Password
    if (!props.password) {
        errors.password = 'فیلد الزامی ';
        isValid = false;
    }
    else if ((props.password.length < 6) || (!passwordRegex.test(props.password))) {
        errors.password = 'پسورد نامعتبر است';
        isValid = false;
    }

    return [errors, isValid];
}

export default Validation