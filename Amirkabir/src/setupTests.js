// jest-dom adds custom jest matchers for asserting on DOM nodes.
// allows you to do things like:
// expect(element).toHaveTextContent(/react/i)
// learn more: https://github.com/testing-library/jest-dom
import '@testing-library/jest-dom';
import { configure } from '@testing-library/react';

configure({
    asyncUtilTimeout: 15000,
});

jest.setTimeout(30000);
const originalError = console.error;

beforeAll(() => {
    console.error = (...args) => {
        if (
            /Warning.*ReactDOM.render is no longer supported/.test(args[0]) ||
            /Warning.*ReactDOMTestUtils.act/.test(args[0]) ||
            /Warning.*defaultProps/.test(args[0]) ||
            /Warning.*act/.test(args[0])
        ) {
            return;
        }
        originalError.call(console, ...args);
    };
});

afterAll(() => {
    console.error = originalError;
});