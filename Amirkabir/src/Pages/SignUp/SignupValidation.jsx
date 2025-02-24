const Validation = (props) => {
    let errors = {};
    let isValid = true;
    let passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
    let emailRegex = /^[a-zA-Z0-9_.±]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$/;
    const phoneRegex = /^09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}$/;

    if(props.SignupInfo)
    {
        //Phone
        if (!props.phone) {
            errors.phone = 'فیلد الزامی ';
            isValid = false;
        }
        else if (!phoneRegex.test(props.phone)) {
            errors.phone = 'شماره تماس نامعتبر است';
            isValid = false;
        }

        //Password
        if (!props.pass) {
            errors.pass = 'فیلد الزامی ';
            isValid = false;
        }
        else if ((props.pass.length < 6) || (!passwordRegex.test(props.pass))) {
            errors.pass = 'پسورد نامعتبر است';
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
        else if(props.pass !== props.confirmPass) {
            errors.confirmPass = 'پسورد و تکرار پسورد مطابقت ندارد';
            isValid = false;
        }
    }
    else
    {
        //Email
        if (!props.email) {
            errors.email = 'فیلد الزامی ';
            isValid = false;
        }
        else if (!emailRegex.test(props.email)) {
            errors.email = 'ایمیل نامعتبر است';
            isValid = false;
        }

        //Code
        if (!props.code) {
            errors.code = 'فیلد الزامی ';
            isValid = false;
        }
    }

    return [errors, isValid];
}

export default Validation