import {Button, Card, Flex, Form, Input, Modal, ModalProps, Popconfirm, Space, Table} from "antd";
import {WidgetTitle} from "@/components/WidgetTitle.tsx";
import {t} from "i18next";
import {DeleteFilled, EditOutlined, PlusOutlined} from "@ant-design/icons";
import {useState} from "react";
import {useMutation, useQuery} from "@tanstack/react-query";
import apiClient from "@/api";

type CommandsViewProps = {
    className?: string;
}


function CommandsView({className}: CommandsViewProps){

    const [createOpen, setCreateOpen] = useState(false)
    const fetchCommands = useFetchCommands()
    const deleteCommand = useDeleteCommand()

    function handleDelete(record) {
        deleteCommand.mutate(record.id, {
            onSuccess: () => {
                fetchCommands.refetch()
            }
        })
    }

    return <Card className={className}>
            <Flex justify={"space-between"}>
                <WidgetTitle>Command</WidgetTitle>
                <Button icon={<PlusOutlined/>} onClick={()=> setCreateOpen(true)}>Add Command </Button>
            </Flex>
        <Table
            className={'mt-2'}
            size={'small'}
            loading={fetchCommands.isLoading}
            dataSource={fetchCommands.data?? []}
            columns={[
            {
                key: "name",
                title: t("Name"),
                dataIndex: "name",
            },
            {
                key: "enable",
                title: t("Enable"),
                dataIndex: "isEnabled",
                width: 100,
                render: (isEnabled) => isEnabled? 'Yes': 'No'
            },
            {
                key: "enable",
                title: '',
                width: 90,
                render: (record)=> <Space>
                    <Button icon={<EditOutlined/>} size={'small'}></Button>
                    <Popconfirm title={'Sure?'} onConfirm={()=> handleDelete(record)}>
                        <Button danger  icon={<DeleteFilled/>} size={'small'} ></Button>
                    </Popconfirm>
                </Space>
            }
        ]}></Table>
        <CreateCommandModel open={createOpen} onCancel={() => setCreateOpen(false)}
            onSuccess={async () => {
                setCreateOpen(false)
               await fetchCommands.refetch()
            }}
        />
    </Card>
}

type CreateCommandProps  = ModalProps & { onSuccess?: () => void }

function CreateCommandModel(props: CreateCommandProps){

    const [form] = Form.useForm();

    const createComamnd = useCreateCommand()

    function handleSubmit(values) {
        createComamnd.mutate({
            name: values.name,
            response: values.response,
        }, {
            onSuccess:()=> {
               if (props.onSuccess)  props?.onSuccess()
            }
        })
    }

    return <Modal {...props} title={'Create Command'} onOk={()=> form.submit()} destroyOnClose={true} >
        <Form layout={'vertical'} onFinish={handleSubmit} form={form} preserve={false} >
            <Form.Item name={'name'} label={t("Name")} rules={[{required: true}]}>
                <Input prefix={"!"}/>
            </Form.Item>
            <Form.Item name={'response'} label={t("Response")} rules={[{required: true}]}>
                <Input.TextArea/>
            </Form.Item>
        </Form>
    </Modal>
}

function useFetchCommands(){
    return useQuery({
        queryKey: ['commands'],
        queryFn: async () => {
            const response = await apiClient.api.listCommands()
            return response.data
        }
    })
}

function useCreateCommand(){
    return useMutation({
        mutationKey: ['create-command'],
        mutationFn: apiClient.api.createCommand
    })
}

function useDeleteCommand(){
    return useMutation({
        mutationKey: ['delete-command'],
        mutationFn: apiClient.api.deleteCommand
    })
}



export default CommandsView;