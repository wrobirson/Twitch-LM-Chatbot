import "./App.css";
import {Button, Col, Result, Row, Spin} from "antd";
import AccountView from "./components/AccountView.tsx";
import {ReactQueryDevtools} from "@tanstack/react-query-devtools";
import {QueryClient, QueryClientConfig, QueryClientProvider} from "@tanstack/react-query";
import ProvidersView from "./components/ProvidersView.tsx";
import PersonalitiesView from "./components/PersonalitiesView.tsx";
import {PermissionsView} from "@/components/PermissionsView.tsx";
import CommandsView from "@/components/CommandsView.tsx";
import {useFetchBroadcasterAccount} from "@/hooks/useFetchBroadcasterAccount.ts";
import env from "@/env.ts";
import {RobotFilled, RobotOutlined, TwitchFilled} from "@ant-design/icons";
import AccountLinkingView from "@/components/AccountLinkingView.tsx";

function EnsureAccountLinked() {

    const fetchAccount = useFetchBroadcasterAccount()

    if (fetchAccount.isLoading) {
        return <Spin/>
    }

    if (!fetchAccount.data) {
       return <div style={{height:'100vh', width: '100vw', display:'flex', justifyContent:'center', alignItems: 'center' }}  >
           <div className={'text-center'}>
               <RobotOutlined className={'text-2xl'} />
               <div className={'text-xl'}>Link you account to start</div>
               <Button className={'mt-3'} size={'large'} type={'primary'} icon={<TwitchFilled/>}
                       href={`${env.API_BASE_URL}/auth/twitch?accountType=0`}>
                   Link your account
               </Button>
           </div>
       </div>
    }

    if (fetchAccount.isSuccess) {
        return <div className="container mx-auto">
            <div className="p-5">
                <Row gutter={16}>
                    <Col md={8}>
                        {/*<AccountView className="mb-4"/>*/}
                        <CommandsView className="mb-4"/>
                        <ProvidersView className="mb-4"/>
                        <PermissionsView className="mb-4"/>
                    </Col>
                    <Col md={16}>
                        <PersonalitiesView/>
                    </Col>
                </Row>
            </div>
        </div>
    }

}

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
                                <AccountView className="mb-4"/>
                                <CommandsView className="mb-4"/>
                                <ProvidersView className="mb-4"/>
                                <PermissionsView className="mb-4"/>
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
