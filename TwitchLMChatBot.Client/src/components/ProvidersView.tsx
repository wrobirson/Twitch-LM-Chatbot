import {DeleteFilled, PlusOutlined} from "@ant-design/icons";
import {Button, Card,  Dropdown, Flex, Popconfirm, Result, Space, Table,} from "antd";
import {useState} from "react";
import {FaComputer} from "react-icons/fa6";
import {PiOpenAiLogoLight} from "react-icons/pi";
import {useFetchProviders} from "@/hooks/useFetchProviders.ts";
import {useDeleteProvider} from "@/hooks/useDeleteProvider.ts";
import {useSetDefaultProvider} from "@/hooks/useSetDefaultProvider.ts";
import {OpenAIConfigModal} from "@/components/OpenAIConfigModal.tsx";
import {LmStudioConfigModal} from "@/components/LmStudioConfigModal.tsx";
import {Provider} from "@/api/types/provider.ts";
import {useTranslation} from "react-i18next";

type ProvidersViewProps={
    className?: string
}

function ProvidersView(props: ProvidersViewProps) {

    const {className = ""} = props;

    const {t} = useTranslation();
    const [openAiModalOpen, setOpenAiModalOpen] = useState(false);
    const [lmStudioOpen, setLmStudioOpen] = useState(false);

    const providersQuery = useFetchProviders();
    const deleteProvider = useDeleteProvider();
    const setDefaultProvider = useSetDefaultProvider();

    const keys =
        providersQuery.data?.filter((a) => a.isDefault)?.map((a) => a.id) ?? [];

    function handleOpenAiModelClose() {
        setOpenAiModalOpen(false);
    }

    function handleDelete(id: number) {
        deleteProvider.mutate(id, {
            onSuccess: () => {
                providersQuery.refetch()
            },
        });
    }

    return (
        <>
            <Card
                className={className}
                loading={providersQuery.isLoading}
                styles={{
                    body: {
                        padding: 0,
                    },
                }}
            >
                {providersQuery.isSuccess && <>
                    <div style={{padding: 24}}>
                        <Flex gap={16} justify="space-between">
                            <div className=" flex-grow-1">
                                <div className="text-2xl font-bold">{t("Providers")}</div>
                            </div>
                            <Dropdown
                                menu={{
                                    items: [
                                        {
                                            key: "lmstudi",
                                            label: t("LM Studio"),
                                            icon: <FaComputer/>,
                                            onClick: () => setLmStudioOpen(true),
                                        },
                                        {
                                            key: "openai",
                                            label: t("Open AI"),
                                            icon: <PiOpenAiLogoLight/>,
                                            onClick: () => setOpenAiModalOpen(true),
                                        },
                                    ],
                                }}
                            >
                                <Button icon={<PlusOutlined/>}>{t("Add")}</Button>
                            </Dropdown>
                        </Flex>
                    </div>
                    <Table<Provider>
                        rowKey={(a) => a.id}
                        pagination={false}
                        rowSelection={{
                            type: "radio",
                            selectedRowKeys: [...keys],
                            onChange(_, selectedRows) {
                                setDefaultProvider.mutate(selectedRows[0].id, {
                                    onSuccess: () => {
                                        providersQuery.refetch()
                                    },
                                });
                            },
                        }}
                        loading={providersQuery.isLoading}
                        dataSource={providersQuery.data}
                        size="small"
                        scroll={{
                            y: 320,
                        }}
                        columns={[
                            {
                                key: "name",
                                title: t("Name"),
                                dataIndex: "name",
                            },
                            {
                                key: "type",
                                title: t("Type"),
                                dataIndex: "typeName",
                                width: 100,
                            },
                            {
                                key: "actions",
                                title: "",
                                width: 60,
                                render: (record) => (
                                    <Space align="center">
                                        <Popconfirm
                                            title={t("Confirm")}
                                            onConfirm={() => handleDelete(record.id)}
                                        >
                                            <Button
                                                icon={<DeleteFilled/>}
                                                size="small"
                                                danger
                                            ></Button>
                                        </Popconfirm>
                                    </Space>
                                ),
                            },
                        ]}
                    />
                </>}
                {providersQuery.error && <Result status={'error'} title={providersQuery.error.message}>
                </Result>}
            </Card>
            <OpenAIConfigModal
                open={openAiModalOpen}
                onClose={handleOpenAiModelClose}
                onCancel={handleOpenAiModelClose}
                onCreated={() => {
                    setOpenAiModalOpen(false);
                    providersQuery.refetch();
                }}
            />
            <LmStudioConfigModal
                open={lmStudioOpen}
                onClose={() => setLmStudioOpen(false)}
                onCancel={() => setLmStudioOpen(false)}
                onCreated={() => {
                    setLmStudioOpen(false);
                    providersQuery.refetch();
                }}
            />
        </>
    );
}

export default ProvidersView;
