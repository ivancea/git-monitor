import React from "react";
import { Route, Routes } from "react-router";
import { Home } from "./components/Home";
import { Layout } from "./components/Layout";

export default function App(): React.ReactElement {
    return (
        <Layout>
            <Routes>
                <Route path="/" element={<Home />} />
            </Routes>
        </Layout>
    );
}
