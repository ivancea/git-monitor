module.exports = {
    settings: {
        react: {
            version: "detect",
        },
        "import/resolver": {
            node: {
                extensions: [".js", ".jsx", ".ts", ".tsx"],
            },
        },
    },
    parser: "@typescript-eslint/parser",
    parserOptions: {
        ecmaVersion: 2018,
        sourceType: "module",
    },
    globals: {
        window: "readonly",
        process: "readonly",
        module: "writable",
    },
    plugins: ["@typescript-eslint"],
    extends: [
        "eslint:recommended",
        "plugin:react/recommended",
        "plugin:import/errors",
        "plugin:import/warnings",
        "plugin:jsx-a11y/recommended",
        "plugin:react-hooks/recommended",
        "plugin:@typescript-eslint/recommended",
        "prettier/@typescript-eslint",
        "plugin:prettier/recommended",
    ],
    rules: {
        "jsx-a11y/click-events-have-key-events": "off",
        "jsx-a11y/no-static-element-interactions": "off",
        "prettier/prettier": [
            "error",
            {
                endOfLine: "auto",
            },
        ],
        "@typescript-eslint/explicit-function-return-type": [
            "error",
            {
                allowExpressions: true,
            },
        ],
        "@typescript-eslint/explicit-module-boundary-types": "off",

        "import/extensions": [
            "error",
            "always",
            {
                js: "never",
                jsx: "never",
                ts: "never",
                tsx: "never",
            },
        ],
        "@typescript-eslint/ban-types": [
            "error",
            {
                extendDefaults: true,
                types: {
                    null: {
                        message: "Use undefined instead",
                        fixWith: "undefined",
                    },
                },
            },
        ],
    },
};
