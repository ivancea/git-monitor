const prod = {
    url: {
        API: "/",
    },
};

const dev = {
    url: {
        API: "https://localhost:5001/",
    },
};

export const config = process.env.NODE_ENV === "development" ? dev : prod;
