import {Dropdown, Modal, Space, Table, TableProps} from "antd";
import {Personality} from "@/api/types/personalities.ts";
import {useFetchPersonalities} from "@/hooks/useFetchPersonalities.ts";
import {useSetDefaultPersonality} from "@/hooks/useSetDefaultPersonality.ts";
import {useDeletePersonality} from "@/hooks/useDeletePersonality.ts";
import {useTranslation} from "react-i18next";
import {CheckOutlined, DeleteFilled} from "@ant-design/icons";
import {useState} from "react";

export function PersonalitiesTable(props: TableProps<Personality> & { onEdit: (record: Personality) => void }) {

    const fetchPersonalities = useFetchPersonalities();
    const setDefault = useSetDefaultPersonality();
    const deletePersonality = useDeletePersonality();
    const {t} = useTranslation()

    const selectedRowKeys =
        fetchPersonalities.data?.filter((a) => a.isDefault).map((a) => a.id) ?? [];

    const [deletingPersonality, setDeletingPersonality] = useState<Personality>()

    function handleDelete(record: Personality) {
        Modal.confirm({
            title: t("Delete personality"),
            content: t("Are you sure?"),
            onOk: () => {
                setDeletingPersonality(record)
                deletePersonality.mutate(record.id, {
                    onSuccess: () => {
                        fetchPersonalities.refetch();
                        setDeletingPersonality(undefined)
                    }
                });
            }
        });
    }

    const handleSelect = (selectedRows: Personality) => {
        setDefault.mutate(selectedRows.id, {
            onSuccess: () => {
                fetchPersonalities.refetch();
            },
        });
    }

    return <Table<Personality>
        loading={fetchPersonalities.isLoading}
        dataSource={fetchPersonalities.data}
        bordered={true}
        pagination={false}
        size={'small'}
        rowKey={(a) => a.id}
        className="mb-0"
        rowSelection={{
            type: "radio",
            selectedRowKeys,
            onChange: (_, selectedRows) => {
                if (selectedRows?.length) {
                    handleSelect(selectedRows[0])
                }
            }
        }}
        scroll={{
            y: window.innerHeight - 220,
        }}
        columns={[
            {
                key: "name",
                title: t("Name"),
                dataIndex: "name",
                width: 200,
                render: (value) => (
                    <Space>
                        <span>{value}</span>
                    </Space>
                ),
            },
            {
                key: "instructions",
                title: t("Instructions"),
                dataIndex: "instructions",
            },
            {
                key: "actions",
                title: "",
                width: 120,
                render: (record) =>
                    <Dropdown.Button
                        loading={deletePersonality.isPending && deletingPersonality?.id == record.id}
                        onClick={() => props.onEdit(record)}
                        menu={{
                            items: [

                                {
                                    label: t('Set as default'),
                                    key: '2',
                                    icon: <CheckOutlined/>,
                                    onClick: () => handleSelect(record)
                                },
                                {
                                    type: 'divider'
                                },
                                {
                                    label: t('Delete'),
                                    key: '3',
                                    icon: <DeleteFilled/>,
                                    danger: true,
                                    onClick: () => {
                                        handleDelete(record);
                                    }
                                },
                            ]
                        }}>
                        {t("Edit")}
                    </Dropdown.Button>

            },
        ]}

    />;
}