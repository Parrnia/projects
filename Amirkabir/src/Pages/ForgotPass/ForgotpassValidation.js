const Validation = (props) => {
    let errors = {};
    let isValid = true;
    let passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;

    //New Password
    if (!props.newPass) {
        errors.newPass = 'فیلد الزامی ';
        isValid = false;
    }
    else if ((props.newPass.length < 6) || (!passwordRegex.test(props.newPass))) {
        errors.newPass = 'پسورد نامعتبر است';
        isValid = false;
    }

    //Confirm Password
    if (!props.confirmPass) {
        errors.confirmPass = 'فیلد الزامی ';
        isValid = false;
    }
    else if ((props.confirmPass.length < 6) || (!passwordRegex.test(props.confirmPass))) {
        errors.confirmPass = 'پسورد نامعتبر است';
        isValid = false;
    }
    else if(props.newPass !== props.confirmPass) {
        errors.confirmPass = 'پسورد و تکرار پسورد مطابقت ندارد';
        isValid = false;
    }

    return [errors, isValid];
}

export default Validation