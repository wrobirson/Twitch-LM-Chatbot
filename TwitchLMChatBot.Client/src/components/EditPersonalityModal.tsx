import {Form, Input, Modal, ModalProps, Space} from "antd";
import {Personality, UpdatePersonalityRequest} from "@/api/types/personalities.ts";
import {useUpdatePersonality} from "@/hooks/useUpdatePersonality.ts";
import {useTranslation} from "react-i18next";

type Props = ModalProps & { onSuccess?: () => void, record: Personality | null }

export function EditPersonalityModal(props: Props) {
    const [form] = Form.useForm();

    const {record, onSuccess} = props

    const {t} = useTranslation();
    const updatePersonality = useUpdatePersonality();

    function handleSubmit(values: UpdatePersonalityRequest) {
        updatePersonality.mutate({id: record!.id, data: values}, {
            onSuccess
        });
    }

    return (
        <Modal
            {...props}
            title={<Space className="text-xl">{t('Edit personality')}</Space>}
            onOk={() => form.submit()}
            maskClosable={false}
            destroyOnClose={true}

            okButtonProps={{
                loading: updatePersonality.isPending,
            }}
        >
            <Form<UpdatePersonalityRequest>
                form={form}
                initialValues={{
                    personalityName: record?.name,
                    instructions: record?.instructions,
                }}
                layout="vertical"
                onFinish={handleSubmit}
                preserve={false}
            >
                <Form.Item
                    name={"personalityName"}
                    label={t("Name")}
                    rules={[{required: true}]}
                >
                    <Input/>
                </Form.Item>

                <Form.Item
                    name={"instructions"}
                    label={t("Instructions")}
                    rules={[{required: true}]}
                >
                    <Input.TextArea rows={10}/>
                </Form.Item>
            </Form>
        </Modal>
    );
}
