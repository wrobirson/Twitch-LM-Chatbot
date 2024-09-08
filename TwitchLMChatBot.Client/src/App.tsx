import "./App.css";
import {Col, Row} from "antd";
import AccountLinkingView from "./components/AccountLinkingView.tsx";
import {ReactQueryDevtools} from "@tanstack/react-query-devtools";
import {QueryClient, QueryClientConfig, QueryClientProvider} from "@tanstack/react-query";
import ProvidersView from "./components/ProvidersView.tsx";
import PersonalitiesView from "./components/PersonalitiesView.tsx";
import {PermissionsView} from "@/components/PermissionsView.tsx";
import CommandsView from "@/components/CommandsView.tsx";

function App() {

    const queryClientConfig: QueryClientConfig = {
        defaultOptions: {
            queries: {
                retry: false,
            },
        },
    }

    return (
        <>
            <QueryClientProvider
                client={new QueryClient(queryClientConfig)}
            >
                <div className="container mx-auto">
                    <div className="p-5">
                        <Row gutter={16}>
                            <Col md={8}>
                                <AccountLinkingView/>
                                <CommandsView  className="mt-4"/>
                                <ProvidersView className="mt-4"/>
                                <PermissionsView className="mt-4"/>
                            </Col>
                            <Col md={16}>
                                <PersonalitiesView/>
                            </Col>
                        </Row>
                    </div>
                </div>
                <ReactQueryDevtools initialIsOpen={false}/>
            </QueryClientProvider>
        </>
    );
}

export default App;
