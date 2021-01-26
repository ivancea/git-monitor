import React from "react";
import { Route } from "react-router";
import { Home } from "./components/Home";
import { Layout } from "./components/Layout";

export default function App(): React.ReactElement {
    return (
        <Layout>
            <Route path="/" component={Home} />
        </Layout>
    );
}
