import {PlusOutlined} from "@ant-design/icons";
import {Button, Card, Col, Flex, Result, Row,} from "antd";
import {useState} from "react";
import {useFetchPersonalities} from "@/hooks/useFetchPersonalities.ts";
import {NewPersonalityModal} from "@/components/NewPersonalityModal.tsx";
import {Personality} from "@/api/types/personalities";
import {EditPersonalityModal} from "@/components/EditPersonalityModal.tsx";
import {useTranslation} from "react-i18next";
import {PersonalitiesTable} from "@/components/PersonalitiesTable.tsx";

function PersonalitiesView() {

    const {t} = useTranslation();

    const [createPersonalityOpen, setCreatePersonalityOpen] = useState(false);
    const [editPersonalityOpen, setEditPersonalityOpen] = useState(false);
    const [editingPersonality, setEditingPersonality] = useState<Personality>();
    const fetchPersonalities = useFetchPersonalities();

    return (
        <Card >
            {fetchPersonalities.error && <Result status={'error'} title={fetchPersonalities.error.message}/>}
            {fetchPersonalities.isSuccess && (<><Flex justify="space-between">
                <div className="text-2xl font-bold">{t("Personalities")}</div>
                <Button
                    icon={<PlusOutlined />}
                    onClick={() => setCreatePersonalityOpen(true)}
                >
                    {t("Add")}
                </Button>
            </Flex>
                <Row className="py-5">
                    <Col md={24}>
                        <PersonalitiesTable
                            onEdit={(record) => {
                                setEditingPersonality(record);
                                setEditPersonalityOpen(true);
                            }} />
                    </Col>
                </Row>
                <NewPersonalityModal
                    open={createPersonalityOpen}
                    onCancel={() => setCreatePersonalityOpen(false)}
                    onSuccess={() => {
                        setCreatePersonalityOpen(false);
                        fetchPersonalities.refetch();
                    }}
                />
                <EditPersonalityModal
                    open={editPersonalityOpen}
                    record={editingPersonality ?? null}
                    onClose={() => setEditPersonalityOpen(false)}
                    onCancel={() => setEditPersonalityOpen(false)}
                    onSuccess={() => {
                        setEditPersonalityOpen(false);
                        fetchPersonalities.refetch();
                    }}
                /></>)}
        </Card>
    );
}

export default PersonalitiesView;
