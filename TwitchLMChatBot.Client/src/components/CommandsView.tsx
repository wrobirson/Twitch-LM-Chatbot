import {
    Button,
    Card,
    Checkbox,
    Flex,
    Form,
    FormProps,
    Input,
    Modal,
    ModalProps,
    Popconfirm,
    Space,
    Table
} from "antd";
import {WidgetTitle} from "@/components/WidgetTitle.tsx";
import {t} from "i18next";
import { DeleteFilled, EditOutlined, PlusOutlined} from "@ant-design/icons";
import {useState} from "react";
import {useMutation, useQuery} from "@tanstack/react-query";
import apiClient from "@/api";
import {Command, CreateCommandRequest, UpdateCommandRequest} from "@/api/generated.ts";

type CommandsViewProps = {
    className?: string;
}

function CommandsView({className}: CommandsViewProps) {

    const [createOpen, setCreateOpen] = useState(false)
    const [updateOpen, setUpdateOpen] = useState(false)
    const [updateCommand, setUpdateCommand] = useState<Command>()

    const fetchCommands = useFetchCommands()
    const deleteCommand = useDeleteCommand()

    function handleDelete(record: Command) {
        deleteCommand.mutate(record.id!, {
            onSuccess: () => {
                fetchCommands.refetch()
            }
        })
    }

    function handleEdit(record: Command) {
        setUpdateCommand(record)
        setUpdateOpen(true)
    }

    return <Card className={className}>
        <Flex justify={"space-between"}>
            <WidgetTitle>Command</WidgetTitle>
            <Button icon={<PlusOutlined/>} onClick={() => setCreateOpen(true)}>Add  </Button>
        </Flex>
        <Table
            className={'mt-2'}
            size={'small'}
            loading={fetchCommands.isLoading}
            dataSource={fetchCommands.data ?? []}
            pagination={false}
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
                    render: (isEnabled) => isEnabled ? 'Yes' : 'No'
                },
                {
                    key: "enable",
                    title: '',
                    width: 90,
                    render: (record) => <Space>
                        <Button icon={<EditOutlined/>} size={'small'} onClick={() => handleEdit(record)}></Button>
                        <Popconfirm title={'Sure?'} onConfirm={() => handleDelete(record)}>
                            <Button danger icon={<DeleteFilled/>} size={'small'}></Button>
                        </Popconfirm>
                    </Space>
                }
            ]}></Table>
        <CreateCommandModal
            open={createOpen}
            onCancel={() => setCreateOpen(false)}
            onSuccess={async () => {
                setCreateOpen(false)
                await fetchCommands.refetch()
            }}
        />
        <UpdateCommandModal
            open={updateOpen}
            command={updateCommand}
            onCancel={() => setUpdateOpen(false)}
            onSuccess={async () => {
                setUpdateOpen(false)
                setUpdateCommand(undefined)
                await fetchCommands.refetch()
            }}/>

    </Card>
}

type CreateCommandProps = ModalProps & { onSuccess?: () => void }

function CommandForm(props: FormProps) {

    const [usingAI, setUsingAI] = useState(props?.initialValues?.usingAI?? true)

    return <Form  {...props} preserve={false} layout={'vertical'}>
        <Form.Item name={'name'} label={t("Name")} rules={[{required: true}]} className={'mb-2'}>
            <Input prefix={"!"}/>
        </Form.Item>

        <Form.Item name={'usingAI'} className={'mb-2'} valuePropName={'checked'}>
            <Checkbox onChange={(e)=> setUsingAI(e.target.checked)}>Respond with AI</Checkbox>
        </Form.Item>

        {!usingAI && (
            <Form.Item name={'response'} label={t("Command response template")}
                       rules={[{required: true}]} className={'mb-2'}>
                <Input.TextArea/>
            </Form.Item>
        )}

        {usingAI && (
            <Form.Item name={'response'} label={t("Instruction for generating the response.")}
                       rules={[{required: true}]} className={'mb-2'}>
                <Input.TextArea/>
            </Form.Item>
        )}

    </Form>;
}

function CreateCommandModal(props: CreateCommandProps) {
    const [form] = Form.useForm();
    const createCommand = useCreateCommand()
    function handleSubmit(values: CreateCommandRequest) {
        createCommand.mutate({
            name: values.name,
            response: values.response,
            usingAI: values.usingAI
        }, {
            onSuccess: () => {
                if (props.onSuccess) props?.onSuccess()
            }
        })
    }
    return <Modal {...props} title={'Create Command'} onOk={() => form.submit()} destroyOnClose={true}>
        <CommandForm
            onFinish={handleSubmit} form={form}
            initialValues={{
                usingAI: true
            }}/>
    </Modal>
}

type UpdateCommandProps = ModalProps & { onSuccess?: () => void, command?: Command }

function UpdateCommandModal(props: UpdateCommandProps) {
    const [form] = Form.useForm();
    const {command, onSuccess} = props;
    const updateCommand = useUpdateCommand()
    function handleSubmit(values: UpdateCommandRequest) {
        updateCommand.mutate({
            id: command!.id!,
            name: values.name,
            response: values.response,
            usingAI: values.usingAI
        }, {
            onSuccess: () => {
                if (onSuccess) onSuccess()
            }
        })
    }

    return <Modal {...props} title={'Create Command'} onOk={() => form.submit()} destroyOnClose={true}>
        {command && (
            <CommandForm
                onFinish={handleSubmit} form={form}
                initialValues={{
                    name: command ? command.name : '',
                    response: command?.response,
                    usingAI: command?.usingAI
                }}/>
        )}
    </Modal>
}

function useFetchCommands() {
    return useQuery({
        queryKey: ['commands'],
        queryFn: async () => {
            const response = await apiClient.api.listCommands()
            return response.data
        }
    })
}

function useCreateCommand() {
    return useMutation({
        mutationKey: ['create-command'],
        mutationFn: apiClient.api.createCommand
    })
}

function useUpdateCommand() {
    return useMutation({
        mutationKey: ['update-command'],
        mutationFn: (data: UpdateCommandRequest & { id: number }) => {
            return apiClient.api.updateCommand(data.id, data)
        }
    })
}


function useDeleteCommand() {
    return useMutation({
        mutationKey: ['delete-command'],
        mutationFn: apiClient.api.deleteCommand
    })
}


export default CommandsView;